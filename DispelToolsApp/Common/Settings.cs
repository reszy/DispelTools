using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;

namespace DispelTools.Common
{
    public static class Settings
    {
        private static string gameRootDir = "";
        private static string outRootDir = "";
        private static IFileSystem fs;

        public static string GameRootDir { get { LoadSettings(); return gameRootDir; } set => SetGameDir(value); }
        public static string OutRootDir { get { LoadSettings(); return outRootDir; } set => SetOutDir(value); }

        public static bool RootsValid { get; private set; }
        private static bool Loaded { get; set; }

        public static IFileSystem FS
        {
            get
            {
                if (fs == null)
                {
                    fs = new FileSystem();
                }
                return fs;
            }
            set => fs = value;
        }

        public static string TranslateToOutDir(string filename)
        {
            if (!FS.Path.IsPathRooted(filename))
            {
                throw new System.ArgumentException("Cannot operate on relative paths");
            }
            if (!RootsValid)
            {
                throw new System.ArgumentException("Root directories not set");
            }
            return filename.Replace(GameRootDir, OutRootDir);
        }

        internal static string GetInitialInputDirectory(string initialDirectory)
        {
            if (string.IsNullOrEmpty(gameRootDir) || initialDirectory.StartsWith(gameRootDir))
            {
                return initialDirectory;
            }
            else
            {
                return gameRootDir;
            }
        }

        private static void SetGameDir(string gameDir)
        {
            if (!FS.Path.IsPathRooted(gameDir))
            {
                throw new System.ArgumentException("Cannot operate on relative paths");
            }
            gameRootDir = gameDir;
            SaveSettings();
            if (!string.IsNullOrEmpty(outRootDir))
            {
                RootsValid = true;
            }
        }

        private static void SetOutDir(string outDir)
        {
            if (!FS.Path.IsPathRooted(outDir))
            {
                throw new System.ArgumentException("Cannot operate on relative paths");
            }
            outRootDir = outDir;
            SaveSettings();
            if (!string.IsNullOrEmpty(gameRootDir))
            {
                RootsValid = true;
            }
        }

        private static string GetSettingsFilePath()
        {
            string exeDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return exeDirectory + "\\DispelTools.cofig";
        }
        private static void LoadSettings()
        {
            string settingsPath = GetSettingsFilePath();
            if (!Loaded && FS.File.Exists(settingsPath))
            {
                try
                {
                    using (var reader = new StreamReader(FS.File.Open(settingsPath, FileMode.Open, FileAccess.Read)))
                    {
                        while (!reader.EndOfStream)
                        {
                            string key = reader.ReadLine();
                            string value = reader.ReadLine();

                            switch (key)
                            {
                                case "gameRootDir": SetGameDir(value); break;
                                case "outRootDir": SetOutDir(value); break;
                            }
                        }
                    }
                }
                catch (IOException)
                {
                    //Skip because settings are not required
                }
                Loaded = true;
            }

        }
        private static void SaveSettings()
        {
            var settings = new Dictionary<string, string>
            {
                { "gameRootDir", gameRootDir },
                { "outRootDir", outRootDir }
            };
            try
            {
                using (var writer = new StreamWriter(FS.File.Open(GetSettingsFilePath(), FileMode.Create, FileAccess.Write)))
                {
                    foreach (var setting in settings)
                    {
                        writer.WriteLine(setting.Key);
                        writer.WriteLine(setting.Value);
                    }
                }
            }
            catch (IOException)
            {
                //Skip because settings are not required
            }
        }
    }
}
