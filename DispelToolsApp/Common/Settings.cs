﻿using DispelTools.DebugTools.MetricTools;
using DispelTools.GameDataModels.Map.Generator;
using System.IO;
using System.IO.Abstractions;

namespace DispelTools.Common
{
    public static partial class Settings
    {
        private static bool debugReadOnlyExtractor = false;
        private static bool copyCommandToClipboard = false;
        private static string gameRootDir = "";
        private static string outRootDir = "";
        private static IFileSystem fs;
        private static GeneratorOptions mapGenerationOptions = new GeneratorOptions();

        public static bool ExtractorReadOnly => debugReadOnlyExtractor;
        public static bool CopyCommandToClipboard { get => copyCommandToClipboard; set { if (value != copyCommandToClipboard) { copyCommandToClipboard = value; SaveSettings(); } } }
        public static string GameRootDir { get { LoadSettings(); return gameRootDir; } set => SetGameDir(value); }
        public static string OutRootDir { get { LoadSettings(); return outRootDir; } set => SetOutDir(value); }
        public static GeneratorOptions MapGenerationOptions { get => mapGenerationOptions; set { if (value != mapGenerationOptions) { mapGenerationOptions = value; SaveSettings(); } } }

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
        public static void LoadSettings()
        {
            string settingsPath = GetSettingsFilePath();
            if (!Loaded && FS.File.Exists(settingsPath))
            {
                var settings = SettingsDto.Default();
                try
                {
                    using (var reader = new StreamReader(FS.File.Open(settingsPath, FileMode.Open, FileAccess.Read)))
                    {
                        while (!reader.EndOfStream)
                        {
                            string key = reader.ReadLine();
                            string value = reader.ReadLine();

                            settings.ParseAndSet(key, value);
                        }
                    }
                }
                catch (IOException)
                {
                    //Skip because settings are not required
                }
                SetAll(settings);
                Loaded = true;
                SaveSettings();
            }
        }
        public static void SaveSettings()
        {
            var settings = new SettingsDto()
            {
                GameRootDir = gameRootDir,
                OutRootDir = outRootDir,
                MapGenerationOptions = MapGenerationOptions.ToSetting(),
                DebugFileMetrics = Metrics.Enabled,
                DebugReadOnlyExtractor = debugReadOnlyExtractor,
                CopyCommandToClipboard = copyCommandToClipboard,
            }.SerializeToMap();

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

        private static void SetGameDir(string gameDir)
        {
            if (!IsValidDirectory(gameDir))
            {
                return;
            }
            if (!FS.Path.IsPathRooted(gameDir))
            {
                throw new System.ArgumentException("Cannot operate on relative paths");
            }
            if (gameRootDir != gameDir)
            {
                gameRootDir = gameDir;
                if (Loaded)
                {
                    SaveSettings();
                }
                if (!string.IsNullOrEmpty(outRootDir))
                {
                    RootsValid = true;
                }
            }
        }

        private static void SetOutDir(string outDir)
        {
            if (!IsValidDirectory(outDir))
            {
                return;
            }
            if (!FS.Path.IsPathRooted(outDir))
            {
                throw new System.ArgumentException("Cannot operate on relative paths");
            }
            if (outRootDir != outDir)
            {
                outRootDir = outDir;
                if (Loaded)
                {
                    SaveSettings();
                }
                if (!string.IsNullOrEmpty(gameRootDir))
                {
                    RootsValid = true;
                }
            }
        }

        private static bool IsValidDirectory(string dir) => !string.IsNullOrEmpty(dir) && FS.Directory.Exists(dir);

        private static string GetSettingsFilePath()
        {
            string exeDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return exeDirectory + "\\DispelTools.config";
        }

        private static void SetAll(SettingsDto settings)
        {
            SetGameDir(settings.GameRootDir);
            SetOutDir(settings.OutRootDir);
            Metrics.Enabled = settings.DebugFileMetrics;
            debugReadOnlyExtractor = settings.DebugReadOnlyExtractor;
            mapGenerationOptions = GeneratorOptions.LoadSetting(settings.MapGenerationOptions);
            copyCommandToClipboard = settings.CopyCommandToClipboard;
        }
    }
}
