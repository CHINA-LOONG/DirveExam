namespace Plugins.Scripts
{
    public static class GameVersion
    {
        public static string Version = "GameTrunk-2013-10-11_14-47-08-3811";
        public static string Branch = "GameTrunk";
        public static string BuildId = "2013-10-11_14-47";

        public static string BundleVersion = "1.0";
        public static string BuildNumber = "20";


        public static string AppVersion
        {
            get
            {
                 return string.Format("{0}.{1}", BundleVersion, BuildNumber);
            }
        }
        public static bool IsMatchVersion(string configKey)
        {
            if (configKey == "default")
                return true;
            var verArray = configKey.Split('_');
            if (verArray.Length != 2)
            {
                return false;
            }

            var appVersion = AppVersion;

            if (CompareVersion(appVersion, verArray[0]) != -1 && //>=
                CompareVersion(appVersion, verArray[1]) != 1) //<=
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Compares the version.
        /// </summary>
        /// <returns> 0等于 1 大于  -1 小于</returns>
        /// <param name="leftVersion">Left version.</param>
        /// <param name="rightVersion">Right version.</param>
        private static int CompareVersion(string leftVersion, string rightVersion)
        {
            var leftData = ParseVersion(leftVersion);
            var rightData = ParseVersion(rightVersion);

            for (int i = 0; i < 3; ++i)
            {
                if (leftData[i] > rightData[i])
                {
                    return 1;
                }
                else if (leftData[i] < rightData[i])
                {
                    return -1;
                }
            }

            return 0;
        }

        private static int[] ParseVersion(string version)
        {
            var verArray = version.Split('.');
            int[] verData = new int[3];
            try
            {
                for (int i = 0; i < 3; ++i)
                {
                    verData[i] = int.Parse(verArray[i]);
                }
            }
            catch
            {
            }

            return verData;
        }
    }
}