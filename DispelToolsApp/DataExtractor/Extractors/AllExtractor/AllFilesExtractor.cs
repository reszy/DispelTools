using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;

namespace DispelTools.DataExtractor.AllExtractor
{
    public class AllFilesExtractor : Extractor
    {
        public AllFilesExtractor()
        {
        }

        public AllFilesExtractor(IFileSystem fs) : base(fs)
        {
        }

        public override void ExtractFile(ExtractionFileProcess process)
        {
            fs.Directory.CreateDirectory(process.OutputDirectory);
            switch (process.Extension.ToLower())
            {
                case ".spr":
                {
                    process.WorkReporter.SetText("Extracting sprites...");
                    new ImageExtractor.SprImageExtractor().ExtractFile(process);
                }
                break;
                case ".map":
                {
                    process.WorkReporter.SetText("Extracting map sprites...");
                    new MapExtractor.MapImageExtractor().ExtractFile(process);
                }
                break;
                case ".snf":
                {
                    process.WorkReporter.SetText("Extracting sounds...");
                    new SoundExtractor.SnfSoundExtractor().ExtractFile(process);
                }
                break;
                default:
                    throw new UnsupportedFileException($"File {process.Filename}{process.Extension} cannot be processed");
            }
        }

        public override List<ExtractionFile> Initialize(List<string> filenames, string outputDirectory)
        {
            string gameDirectory = filenames[0];
            string[] fileExtensions = new string[] { ".spr", ".snf", ".map", ".btl", ".gtl" };

            return GetAllFiles(gameDirectory)
                .Where(file => fileExtensions.Contains(fs.Path.GetExtension(file.ToLower())))
                .OrderBy(file => fs.Path.GetExtension(file).ToLower())
                .Select(file => new ExtractionFile(file, CreateOutputDirectoryName(file.Replace(gameDirectory, outputDirectory))))
                .ToList();
        }

        private string CreateOutputDirectoryName(string path)
        {
            string[] mapFileExtensions = new string[] { ".map", ".btl", ".gtl" };
            string directory = fs.Path.GetDirectoryName(path);
            if (mapFileExtensions.Contains(fs.Path.GetExtension(path)))
            {
                return fs.Path.Combine(directory, fs.Path.GetFileName(path).Replace(".", "_"));
            }
            else
            {
                return directory;
            }
        }

        public override ExtractorValidationResult Validate(List<string> filenames, out string message)
        {
            var gameDirectory = filenames[0];
            if (!IsDispelDirectory(gameDirectory))
            {
                message = $"\"{gameDirectory}\" is not Dispel game directory.";
                return ExtractorValidationResult.Warning;
            }
            return base.Validate(filenames, out message);
        }

        private bool IsDispelDirectory(string gameDirectory)
        {
            string[] filesInDirectory = fs.Directory.GetFiles(gameDirectory);
            string[] directoriesInDirectory = fs.Directory.GetDirectories(gameDirectory);
            return filesInDirectory.Select(path => fs.Path.GetFileName(path))
                .Where(file => file == "Dispel.exe" || file == "AllMap.ini")
                .Count() == 2
                &&
                directoriesInDirectory.Select(path => fs.Path.GetFileName(path))
                .Where(dir => dir == "CharacterInGame" || dir == "Main")
                .Count() == 2;
        }

        private List<string> GetAllFiles(string directory, int maxLevel = 4)
        {
            if (maxLevel == 0)
            {
                return new List<string>();
            }
            var files = fs.Directory.GetFiles(directory).ToList();
            string[] directories = fs.Directory.GetDirectories(directory);

            foreach (string downDirectory in directories)
            {
                files.AddRange(GetAllFiles(downDirectory, maxLevel - 1));
            }
            return files;
        }
    }
}
