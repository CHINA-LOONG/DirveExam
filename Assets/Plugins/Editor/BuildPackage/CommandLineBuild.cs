using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Plugins.Scripts;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Plugins.Editor.BuildPackage
{
    public static class CommandLineBuild
    {
        private const string ArgsBuildLocation = "-buildlocation";
        private const string ArgsUpdateVersion = "-updateversion";
        private const string ArgsAndroidSdkRoot = "-androidsdkroot";

        [MenuItem(@"Window/Build/Build iOS")]
        public static void BuildiOS()
        {
            BuildTarget(UnityEditor.BuildTarget.iOS, "Build/Build-iOS", BuildOptions.None);
        }

        [MenuItem(@"Window/Build/Export iOS")]
        public static void ExportiOS()
        {
            BuildTarget(UnityEditor.BuildTarget.iOS, "Build/Build-iOS", BuildOptions.None);
        }

        [MenuItem(@"Window/Build/Build Android")]
        public static void BuildAndroid()
        {
            BuildTarget(UnityEditor.BuildTarget.Android, "Build/Build-Android.apk", BuildOptions.None);
        }

        [MenuItem(@"Window/Build/Export Android")]
        public static void ExportAndroid()
        {
            EditorUserBuildSettings.exportAsGoogleAndroidProject = true;
            EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;

            BuildTarget(UnityEditor.BuildTarget.Android, "Build/Build-Android",
                BuildOptions.AcceptExternalModificationsToPlayer);
        }

        private static void BuildTarget(BuildTarget buildTarget, string defaultLocation, BuildOptions options)
        {
            if (HasArgument(ArgsUpdateVersion))
            {
                GenerateGameVersion();
            }

            if (buildTarget == UnityEditor.BuildTarget.Android)
            {
                var androidSdkRoot = GetArgument(ArgsAndroidSdkRoot);
                if (androidSdkRoot.Length > 0)
                {
                    AndroidSdkRoot = androidSdkRoot;
                }
            }

            var scenes = CollectBuildScenes();

            var locationPathName = GetBuildLocation(buildTarget, defaultLocation);

            BuildPipeline.BuildPlayer(scenes, locationPathName, buildTarget, options);
        }

        [MenuItem(@"Window/Build/Export Assets Package")]
        public static void ExportAssetsPackage()
        {
            var info = Directory.GetParent(Application.dataPath);

            var filename = string.Format("{0}.unitypackage", Application.productName);
            filename = Path.Combine(info.FullName, filename);

            AssetDatabase.ExportPackage("Assets", filename,
                ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Recurse);
        }

        [MenuItem("Window/Build/Generate GameVersion")]
        public static int GenerateGameVersion()
        {
            return GenerateGameVersion(false);
        }

        [MenuItem("Window/Build/Generate GameVersion and Commit")]
        public static int GenerateGameVersionAndComit()
        {
            return GenerateGameVersion(true);
        }

        public static int GenerateGameVersion(bool commit)
        {
            var projectPath = Directory.GetParent(Application.dataPath).FullName;
            var local = CommandHelper.SvnRevision(projectPath);
            var head = CommandHelper.SvnRevision(projectPath, "HEAD");
            if (local != head)
            {
                Debug.LogWarningFormat(
                    "Current project is not up to date. You should update first before set version. local: {0}, remote {1} ",
                    local, head);
            }

            var lastLog = CommandHelper.SvnLogRemote(projectPath);
            if (lastLog.StartsWith("BUILD VERSION NUMBER"))
            {
                Debug.LogWarningFormat("BUILD VERSION NUMBER has been updated\n{0}", lastLog);
                return 0;
            }

            var version = ModifyVersionNumber(local, "BUILD VERSION NUMBER", commit);

            AssetDatabase.Refresh();

            return version;
        }

        [MenuItem("Window/Build/Generate Release Versions")]
        public static void BuildVersionNumber()
        {
            PrepareBuildSettings();
        }

        private static void PrepareBuildSettings(string productName = null, string identifier = null)
        {
            if (!string.IsNullOrEmpty(productName))
            {
                PlayerSettings.productName = productName;
            }

            if (!string.IsNullOrEmpty(identifier))
            {
                PlayerSettings.applicationIdentifier = identifier;
            }

            try
            {
                var svnVersion = GenerateGameVersion();

                // we need to skip this version number;
                if (svnVersion % 2 != 0)
                {
                    svnVersion += 1;
                    if (svnVersion != ModifyVersionNumber(svnVersion, "SKIP VERSION NUMBER"))
                    {
                        return;
                    }
                }

                // set version number for DEVELOPMENT VERSION NUMBER
                svnVersion += 1;
                if (svnVersion != ModifyVersionNumber(svnVersion, "DEVELOPMENT VERSION NUMBER"))
                {
                    return;
                }

                // set version number for PUBLISH VERSION NUMBER
                svnVersion += 1;
                if (svnVersion != ModifyVersionNumber(svnVersion, "PUBLISH VERSION NUMBER"))
                {
                    return;
                }

                Debug.LogFormat("GameVersion changed: {0}, All Done!", svnVersion);
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("Prepare Build Settings Error: {0}", e.Message);
            }

            AssetDatabase.Refresh();
        }

        private static string[] CollectBuildScenes()
        {
            var scenes = new List<string>();

            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene == null)
                    continue;
                if (scene.enabled)
                    scenes.Add(scene.path);
            }

            return scenes.ToArray();
        }

        static bool HasArgument(string argument)
        {
            var args = Environment.GetCommandLineArgs();
            var index = Array.IndexOf(args, argument);
            return index >= 0;
        }

        static string GetArgument(string argument)
        {
            var args = Environment.GetCommandLineArgs();
            var index = Array.IndexOf(args, argument);
            if (index < 0 || index == args.Length - 1)
            {
                return "";
            }

            index++;
            Debug.Log(string.Format("GetArgument for {0} return {1}", argument, args[index]));
            return args[index];
        }

        static string GetBuildLocation(BuildTarget buildTarget, string defaultLocation)
        {
            var args = Environment.GetCommandLineArgs();
            var indexOfBuildLocation = Array.IndexOf(args, ArgsBuildLocation);
            if (indexOfBuildLocation >= 0)
            {
                indexOfBuildLocation++;
                Debug.Log(string.Format("Build Location for {0} set to {1}", buildTarget, args[indexOfBuildLocation]));
                return args[indexOfBuildLocation];
            }

            var location = EditorUserBuildSettings.GetBuildLocation(buildTarget);
            location = defaultLocation.Length != 0 ? defaultLocation : location;
            Debug.Log(string.Format("Build Location for {0} not set. Defaulting to {1}", buildTarget, location));
            return location;
        }

        private static int ModifyVersionNumber(int svnVersion, string logBase, bool commit = false)
        {
            var projectRoot = Directory.GetParent(Application.dataPath).FullName + Path.DirectorySeparatorChar;

            var pathGameVersion = CommandHelper.GetFileFullName("GameVersion.cs");
            var pathProjectSettings =
                CommandHelper.GetFileFullName("ProjectSettings.asset", projectRoot + "ProjectSettings");

            var contentGameVersion = File.ReadAllText(pathGameVersion, Encoding.UTF8);
            var contentProjectSettings = File.ReadAllText(pathProjectSettings, Encoding.UTF8);

            // Sync Bundle Version
            var m = Regex.Match(contentGameVersion, "BundleVersion.*\"(.*)\";");
            var bundleVersionG = m.Groups[1].Value.Trim();

            m = Regex.Match(contentProjectSettings, "bundleVersion:\\s*(.*)");
            var bundleVersionS = m.Groups[1].Value.Trim();

            var bundleVersion = string.CompareOrdinal(bundleVersionG, bundleVersionS) > 0
                ? bundleVersionG
                : bundleVersionS;

            //BundleVersion = Regex.Replace(BundleVersion, @"(\d+.\d+).\d+", "$1." + svnVersion);

            contentGameVersion =
                Regex.Replace(contentGameVersion, "(BundleVersion.*)\".*\"", "$1\"" + bundleVersion + '"');
            contentProjectSettings =
                Regex.Replace(contentProjectSettings, "(bundleVersion:) .*", "$1 " + bundleVersion);

            // change BuildNumber iOS
            contentGameVersion = Regex.Replace(contentGameVersion, "(BuildNumber.*)\".*\"", "$1\"" + svnVersion + '"');
            contentProjectSettings = Regex.Replace(contentProjectSettings, @"(buildNumber:(.*\r*\n*){4}.*iOS:).*$",
                "$1 " + svnVersion, RegexOptions.Multiline);
            contentProjectSettings = Regex.Replace(contentProjectSettings,
                @"(buildNumber:(.*\r*\n*){4}.*Standalone:).*$", "$1 " + svnVersion, RegexOptions.Multiline);
            contentProjectSettings =
                Regex.Replace(contentProjectSettings, "(AndroidBundleVersionCode:).*", "$1 " + svnVersion);

            var utf8WithoutBOM = new UTF8Encoding(false);

            File.WriteAllText(pathGameVersion, contentGameVersion, utf8WithoutBOM);
            File.WriteAllText(pathProjectSettings, contentProjectSettings, utf8WithoutBOM);

            // Modify Global config
            var pathGlobalConfig = CommandHelper.GetFileFullName("GlobalConfig.prefab");
            var contentGlobalConfig = File.ReadAllText(pathGlobalConfig, Encoding.UTF8);
            contentGlobalConfig = Regex.Replace(contentGlobalConfig, "(resVersion:) .*", "$1 " + svnVersion);
            File.WriteAllText(pathGlobalConfig, contentGlobalConfig, utf8WithoutBOM);

            // commit files to svn.
            if (commit)
            {
                var arguments = string.Format("commit -m \"{0}: {1}.{2}\" \"{3}\" \"{4}\" \"{5}\"",
                    logBase, bundleVersion, svnVersion, pathProjectSettings.Replace(projectRoot, ""),
                    pathGameVersion.Replace(projectRoot, ""), pathGlobalConfig.Replace(projectRoot, ""));

                // UnityEngine.Debug.Log("myProcess.StartInfo.Arguments: " + arguments);
                string output;

                CommandHelper.Process("pwd", "", out output);

                CommandHelper.Process("svn", arguments, out output);
                m = Regex.Match(output, "revision (\\d+)");
                if (m.Success && m.Groups.Count > 0)
                {
                    var committedRevision = int.Parse(m.Groups[1].Value.Trim());

                    if (committedRevision != svnVersion)
                    {
                        Debug.LogWarningFormat("Modify Version Number Error: {0}", output);
                        return 0;
                    }
                }
            }

            Debug.LogFormat("Set {0}: {1}.{2}", logBase, bundleVersion, svnVersion);

            return svnVersion;
        }

        public static string AndroidSdkRoot
        {
            get
            {
                return EditorPrefs.GetString("AndroidSdkRoot");
            }
            set
            {
                EditorPrefs.SetString("AndroidSdkRoot", value);
            }
        }
    }
}