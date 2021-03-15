using DispelTools.DataExtractor.ExtractionStatus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DispelTools.DataExtractor.AllExtractor
{
    public class AllFilesExtractor : Extractor
    {
        public override void ExtractFile(ExtractionFileProcess process)
        {
            Directory.CreateDirectory(process.OutputDirectory);
            switch (process.Extension.ToLower())
            {
                case ".spr":
                {
                    process.Extractor.ReportNewStatus(StatusNameChanged.NewStatusInProgress("Extracting sprites"));
                    new ImageExtractor.SprImageExtractor().ExtractFile(process);
                }
                break;
                case ".map":
                {
                    process.Extractor.ReportNewStatus(StatusNameChanged.NewStatusInProgress("Extracting map sprites"));
                    new ImageExtractor.SprImageExtractor().ExtractFile(process);
                }
                break;
                case ".snf":
                {
                    process.Extractor.ReportNewStatus(StatusNameChanged.NewStatusInProgress("Extracting sounds"));
                    new SoundExtractor.SnfSoundExtractor().ExtractFile(process);
                }
                break;
                default:
                    throw new ArgumentException($"File {process.Filename}{process.Extension} cannot be processed");
            }
        }

        public override List<ExtractionFile> Initialize(ExtractionManager extractionManager, List<string> filenames, string outputDirectory)
        {
            extractionManager.ReportNewStatus(StatusNameChanged.NewStatusInProgress("Scanning"));
            string gameDirectory = filenames[0];
            string[] fileExtensions = new string[] { ".spr", ".snf", ".map", ".btl", ".gtl" };
            if (isDispelDirectory(gameDirectory))
            {
                return GetAllFiles(gameDirectory)
                    .Where(file => fileExtensions.Contains(Path.GetExtension(file)))
                    .OrderBy(file => Path.GetExtension(file).ToLower())
                    .Select(file => new ExtractionFile(file, CreateOutputDirectoryName(file.Replace(gameDirectory, outputDirectory))))
                    .ToList();
            }
            else
            {
                MessageBox.Show($"\"{gameDirectory}\" is not Dispel game directory");
                return new List<ExtractionFile>();
            }
        }

        private string CreateOutputDirectoryName(string path)
        {
            string[] mapFileExtensions = new string[] { ".map", ".btl", ".gtl" };
            string directory = Path.GetDirectoryName(path);
            if (mapFileExtensions.Contains(Path.GetExtension(path)))
            {
                return Path.Combine(directory, Path.GetFileName(path).Replace(".", "_"));
            }
            else
            {
                return directory;
            }
        }

        private bool isDispelDirectory(string gameDirectory)
        {
            string[] filesInDirectory = Directory.GetFiles(gameDirectory);
            string[] directoriesInDirectory = Directory.GetDirectories(gameDirectory);
            return filesInDirectory.Select(path => Path.GetFileName(path))
                .Where(file => file == "Dispel.exe" || file == "AllMap.ini")
                .Count() == 2
                &&
                directoriesInDirectory.Select(path => Path.GetFileName(path))
                .Where(dir => dir == "CharacterInGame" || dir == "Main")
                .Count() == 2;
        }

        private List<string> GetAllFiles(string directory, int maxLevel = 4)
        {
            if (maxLevel == 0)
            {
                return new List<string>();
            }
            var files = Directory.GetFiles(directory).ToList();
            string[] directories = Directory.GetDirectories(directory);

            foreach (string downDirectory in directories)
            {
                files.AddRange(GetAllFiles(downDirectory, maxLevel - 1));
            }
            return files;
        }
    }
}
