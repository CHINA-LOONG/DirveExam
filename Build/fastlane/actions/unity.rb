module Fastlane
    module Actions
        class UnityAction < Action
            def self.get_unity_path(params)
                UI.header '' "Detect Unity executable path. Find all installed unitys installed in
            /Applications ~/Applications and /Applications/Unity/Hub
            This will return the unity which match the ProjectSettings/ProjectVersion.txt required.
            If no matched version, this will return the lastest installed unity." ''

                fh = open "#{params[:project_path].strip}/ProjectSettings/ProjectVersion.txt"
                content = fh.read
                fh.close

                required_unity_version = content.split(' ').last

                UI.important "required unity version: #{required_unity_version}"

                # Get all installed Unity in /Applications, /Users/xykong/Applications
                unity_path = [
                    '/Applications',
                    '~/Applications',
                    '/Applications/Unity/Hub'
                ].join(' ')

                unity_list = `find #{unity_path} -maxdepth 4 -name "Unity*.app" 2>/dev/null | grep -v Reporter`.split("\n")

                unitys = {}

                for item in unity_list

                    plist = item + '/Contents/Info.plist'
                    next unless File.file?(plist)

                    fh = open plist
                    content = fh.read
                    fh.close

                    # UI.message "open file : #{plist}"

                    m = content.match(/Unity version ([^(]*) /)

                    # UI.message "captures: #{m.captures}"

                    next if m.nil? || m.captures.empty?

                    unitys[m.captures[0]] = item
                end

                puts(Terminal::Table.new(
                         title: 'Installed unitys'.yellow,
                         rows: FastlaneCore::PrintTable.transform_output(unitys)
                     ))

                if unitys.empty?
                    UI.error 'No installed unity detected.'
                    return ''
                end

                if unitys[required_unity_version]
                    return unitys[required_unity_version] + '/Contents/MacOS/Unity'
                end

                unitys[unitys.keys.max] + '/Contents/MacOS/Unity'
            end

            def self.log_checking(params)
                logfile = params[:logfile]

                UI.important("Content checking: #{logfile}")
                log_content = `cat #{logfile}`

                if log_content.include? 'Multiple Unity instances'
                    UI.user_error!("
                        It looks like another Unity instance is running with this project open.
                        Multiple Unity instances cannot open the same project.
                        #{params[:project_path].strip}
                        Instance: #{`cat ../Library/EditorInstance.json`.delete("\n")}
                    ")
                end

                if ['Scripts have compiler errors.',
                    'Compilation failed:'].any? { |needle| log_content.include? needle }

                    UI.user_error!("
                        #{params[:project_path].strip}
                        Scripts have compiler errors.
                        \n#{`cat #{logfile} | grep -i ' error\\b' | sort | uniq`}
                    ")
                end
            end

            def self.run(params)
                beginning_time = Time.now
                executable = get_unity_path(params)
                executable = params[:executable].strip if executable == ''
                params[:executable] = executable

                UI.important "use unity executable: #{executable}"

                logfile = (params[:logfile] || '~/Library/Logs/Unity/Editor.log').strip
                params[:logfile] = logfile

                build_cmd = executable.to_s
                build_cmd << " -projectPath #{params[:project_path].strip}"
                build_cmd << ' -batchmode'
                build_cmd << ' -quit'
                build_cmd << ' -nographics' if params[:nographics]
                build_cmd << " -executeMethod #{params[:execute_method]}" if params[:execute_method]
                build_cmd << ' -runEditorTests' if params[:run_editor_tests]
                build_cmd << " -resultsFileDirectory=#{params[:results_file_directory]}" if params[:results_file_directory]
                build_cmd << " #{params[:xcargs]}" if params[:xcargs]
                build_cmd << " -logfile #{' 2>&1 | tee ' if params[:use_tee]}#{params[:logfile].strip}" if params[:logfile]

                UI.message ''
                puts(Terminal::Table.new(
                         title: 'Unity'.green,
                         headings: %w[Option Value],
                         rows: FastlaneCore::PrintTable.transform_output(params.values)
                     ))
                UI.message ''

                UI.important 'Start running'
                UI.important "Check out logs at #{logfile} if the build failed"
                UI.message ''

                result = ''
                job2 = Thread.new do
                    result = sh(
                        build_cmd,
                        error_callback: lambda { |_result|
                                            raise 'Unity failed with a non-zero exit status'
                                        }
                    )
                end

                job1 = Thread.new do
                    loop do # inifinite loop
                        break if File.file?(logfile)
                        sleep 0.1
                    end

                    sleep 3.0

                    open(logfile, 'r') do |file|
                        file.seek(0, IO::SEEK_END) # rewinds file to the end
                        loop do # inifinite loop
                            changes = file.read
                            # UI.message changes unless changes.empty?

                            if changes.include? 'Receiving unhandled NULL exception'
                                raise 'Unity: Receiving unhandled NULL exception'
                            end

                            sleep 1.0 # sleep for a second; without it script would use 100% of processor

                            break unless job2.alive?
                        end
                    end
                end

                job1.join
                job2.join

                log_checking(params)
                UI.success 'Completed'

                result
            rescue StandardError => ex
                Thread.kill(job1)
                Thread.kill(job2)
                # kill unity instance
                pid = `cat ../Library/EditorInstance.json 2>&1 | grep process_id | awk -F ':' '{print $2}'`.strip
                Process.kill('KILL', pid.to_i) unless pid.empty?

                UI.important("Unity failed: #{ex}")

                log_checking(params)
            #ensure
                UI.important "Time elapsed #{Time.now - beginning_time} seconds"
            end

            #####################################################
            # @!group Documentation
            #####################################################

            def self.description
                'Run Unity in batch mode'
            end

            def self.available_options
                [
                    FastlaneCore::ConfigItem.new(
                        key: :executable,
                        env_name: 'FL_UNITY_EXECUTABLE',
                        description: 'Path for Unity executable, use only if automatic detecte failed',
                        default_value: '/Applications/Unity/Unity.app/Contents/MacOS/Unity'
                    ),

                    FastlaneCore::ConfigItem.new(
                        key: :project_path,
                        env_name: 'FL_UNITY_PROJECT_PATH',
                        description: 'Path for Unity project',
                        default_value: Dir.pwd.to_s
                    ),

                    FastlaneCore::ConfigItem.new(
                        key: :execute_method,
                        env_name: 'FL_UNITY_EXECUTE_METHOD',
                        description: 'Method to execute',
                        optional: true,
                        default_value: nil
                    ),

                    FastlaneCore::ConfigItem.new(
                        key: :nographics,
                        env_name: 'FL_UNITY_NOGRAPHICS',
                        description: 'Initialize graphics device or not',
                        is_string: false,
                        default_value: true
                    ),

                    FastlaneCore::ConfigItem.new(
                        key: :use_tee,
                        env_name: 'FL_TEE_LOG',
                        description: 'use tee or not. The tee utility copies standard input to standard output, making a copy in zero or more files.  The output is unbuffered',
                        optional: true,
                        is_string: false,
                        default_value: false
                    ),

                    FastlaneCore::ConfigItem.new(
                        key: :logfile,
                        env_name: 'FL_UNITY_LOGFILE',
                        description: 'Specify where the Editor or Windows/Linux/OSX standalone log file are written. if none then redirect to stdout',
                        optional: true,
                        default_value: nil
                    ),

                    FastlaneCore::ConfigItem.new(
                        key: :run_editor_tests,
                        env_name: 'FL_UNITY_RUN_EDITOR_TESTS',
                        description: 'Option to run editor tests',
                        is_string: false,
                        default_value: false
                    ),

                    FastlaneCore::ConfigItem.new(
                        key: :results_file_directory,
                        env_name: 'FL_RESULTS_FILE_DIRECTORY',
                        description: 'Path for integration test results',
                        optional: true,
                        default_value: nil
                    ),

                    FastlaneCore::ConfigItem.new(
                        key: :xcargs,
                        env_name: 'FL_XCARGS',
                        description: 'Pass additional arguments to unity for the build phase',
                        optional: true,
                        default_value: nil
                    )
                ]
            end

            def self.authors
                ['dddnuts']
            end

            def self.is_supported?(platform)
                %i[ios android].include?(platform)
            end
        end
    end
end
