using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Xml;
using UnityEngine;

namespace Plugins.Scripts
{
    public static class CommandHelper
    {
        public static string NormalizePath(string path)
        {
            //path = path.Replace("/", "\\");
            return Path.GetFullPath(new Uri(path).LocalPath)
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        public static int Process(string fileName, string arguments, out string output)
        {
#if UNITY_EDITOR_OSX
            var pathvar = Environment.GetEnvironmentVariable("PATH");
            Environment.SetEnvironmentVariable("PATH", "/usr/local/bin:" + pathvar);
            Environment.SetEnvironmentVariable("LC_ALL", "C");
            Environment.SetEnvironmentVariable("LC_CTYPE", "en_US.UTF-8");
#endif

            var procStartInfo = new ProcessStartInfo(Environment.ExpandEnvironmentVariables(fileName)
                , Environment.ExpandEnvironmentVariables(arguments))
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = new Process
            {
                StartInfo = procStartInfo,
                EnableRaisingEvents = true
            };

            output = "";

            try
            {
                process.Start();
                output += process.StandardOutput.ReadToEnd();
                output += process.StandardError.ReadToEnd();
                process.WaitForExit();
                return process.ExitCode;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex);
            }

            return -1;
        }

        public static string GetSvnPath()
        {
            const string cmdPath = "%SystemRoot%\\system32\\cmd.exe";
            string output;

            Process(cmdPath, "/c where svn", out output);
            return output.Trim();
        }

        public static bool SvnIsUpToDate(string path)
        {
            var local = SvnRevision(path);
            var head = SvnRevision(path, "HEAD");

            return local == head;
        }

        public static int SvnRevision(string path, string revision = null)
        {
            string result;

            path = path.Replace("\\", "/");
            var arguments = string.IsNullOrEmpty(revision)
                ? "info --xml "
                : string.Format("info --xml -r {0} ", revision);
            var ret = Process("svn", arguments + path, out result);

            if (ret == 0)
            {
                var doc = new XmlDocument();
                doc.LoadXml(result);
                if (doc.DocumentElement != null)
                {
                    var revisionNode = doc.DocumentElement.SelectSingleNode("//commit/@revision");
                    int version;
                    if (revisionNode != null && int.TryParse(revisionNode.InnerText, out version))
                    {
                        return version;
                    }
                }
            }

            return 0;
        }

        public static string SvnLogRemote(string path)
        {
            string result;

            path = path.Replace("\\", "/");

            var arguments = string.Format(
                "-c \" svn log ` svn info {0} | grep ^URL | cut -f2 -d' ' ` -l1 | head -n4 | tail -n1 \"",
                path);

            Process("bash", arguments, out result);

            return result;
        }

        public static string GitComit(string path)
        {
            string result;

            path = path.Replace("\\", "/");
            var ret = Process("git", "log --pretty=format:\" % H\" -n 1 " + path, out result);

            UnityEngine.Debug.Log(result);

            if (ret == 0)
            {
                return result;
            }

            return result;
        }

        // ReSharper disable once UnusedMethodReturnValue.Local
        private static IEnumerator ProcessDownload(WWW www)
        {
            yield return www;
        }

        private static void Download(WWW www)
        {
            // ReSharper disable once IteratorMethodResultIsIgnored
            ProcessDownload(www);
            while (!www.isDone)
            {
            }
        }

        public static string GetFileFullName(string searchPattern, string root = null)
        {
            if (string.IsNullOrEmpty(root))
            {
                root = Application.dataPath;
            }

            foreach (var file in new DirectoryInfo(root).GetFiles(searchPattern, SearchOption.AllDirectories))
            {
                //UnityEngine.Debug.Log(file.FullName);

                return file.FullName;
            }

            return string.Empty;
        }

        public static void CopyFromStreamingAssets(string sourceName, string toName = null)
        {
            if (string.IsNullOrEmpty(toName))
            {
                toName = sourceName;
            }

            // ok , this is first time application start!
            // so lets copy prebuild database from StreamingAssets and load store to persistancePath with Test2
            // ReSharper disable once JoinDeclarationAndInitializer
            // ReSharper disable once RedundantAssignment
            byte[] bytes = null;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
            var dbpath = "file://" + Application.streamingAssetsPath + "/" + sourceName;
            var www = new WWW(dbpath);
            Download(www);
            bytes = www.bytes;
#elif UNITY_WEBPLAYER
			string dbpath = "StreamingAssets/" + sourceName;
            WWW www = new WWW(dbpath);
            Download(www);
            bytes = www.bytes;
#elif UNITY_IPHONE
			string dbpath = Application.dataPath + "/Raw/" + sourceName;
            try
            {	
                using ( FileStream fs = new FileStream(dbpath, FileMode.Open, FileAccess.Read, FileShare.Read) )
                {
                    bytes = new byte[fs.Length];
                    fs.Read(bytes,0,(int)fs.Length);
                    fs.Close();
                }			
            }
            catch (Exception e)
            {
				UnityEngine.Debug.LogError("CopyFromStreamingAssets Fail with Exception: " + e);
            }
#elif UNITY_ANDROID
			string dbpath = Application.streamingAssetsPath + "/" + sourceName;
            WWW www = new WWW(dbpath);
            Download(www);
            bytes = www.bytes;
#endif

            if (bytes == null)
            {
                return;
            }

            try
            {
                var filePath = Application.persistentDataPath + "/" + toName;
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // Copy database to real file into cache folder
                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();
                    UnityEngine.Debug.Log("Copy file from StreamingAssets to persistentDataPath: " + filePath);
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("CopyFromStreamingAssets Fail with Exception: " + e);
            }
        }
    }
}