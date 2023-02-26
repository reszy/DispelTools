using DispelTools.Common;
using DispelTools.DataEditor.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DispelTools.DataEditor.Export
{
    public class ExportAsDocScheme
    {
        private readonly IFileSystem fs;
        private readonly Dictionary<string, MapperDefinition> filenames = new();

        public ExportAsDocScheme(IFileSystem fs)
        {
            this.fs = fs;
        }

        public void PrepareDirectoryAndMappers()
        {
            var outputDirectory = FindDocsDirectory();
            var mappers = FindAllMappers();
            foreach (var mapper in mappers)
            {
                var filename = ConvertMapperNameToFilename(mapper.GetMapperName()) + ".txt";
                filenames[fs.Path.Combine(outputDirectory, filename)] = mapper;
            }
        }

        internal void Setup(Dictionary<string, MapperDefinition> mappers)
        {
            foreach(var mapperEntry in mappers)
            {

                filenames[mapperEntry.Key] = mapperEntry.Value;
            }
        }

        private string FindDocsDirectory()
        {
            string searchPattern = @"Docs\Game\Files";
            string searchRoot = fs.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? string.Empty;
            string? foundDirectory = null;
            for (int i = 0; i < 7; i++)
            {
                var path = fs.Path.Combine(searchRoot, searchPattern);
                if (fs.Directory.Exists(path))
                {
                    foundDirectory = path;
                    break;
                }
                else
                {
                    searchRoot = fs.Path.Combine(searchRoot, "..");
                }
            }
            if (foundDirectory is null) throw new ExportException($"Cannot export to project. Cannot find directory {searchPattern}");
            return foundDirectory;
        }

        private static List<MapperDefinition> FindAllMappers()
        {
            var mappers = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => string.Equals(t.Namespace, "DispelTools.DataEditor.Mappers", StringComparison.Ordinal));

            List<MapperDefinition> result = new();
            foreach (var mapperType in mappers)
            {
                var mapper = (MapperDefinition?)mapperType.GetConstructor(Array.Empty<Type>())?.Invoke(Array.Empty<object>());
                if (mapper is not null) result.Add(mapper);
            }
            return result;
        }

        public void Export()
        {
            foreach (var file in filenames)
            {
                Export(file.Key, file.Value);
            }
        }
        private void Export(string filename, MapperDefinition mapper)
        {
            using var file = fs.File.Create(filename);
            using var stringWriter = new StreamWriter(file);
            SchemeConverter schemeConverter = new(mapper);
            var stringBuilder = schemeConverter.ToTxt(fs.Path.GetFileNameWithoutExtension(filename));
            stringWriter.Write(stringBuilder);
        }

        private static string ConvertMapperNameToFilename(string mapperName) => mapperName.Replace("Mapper", "", StringComparison.OrdinalIgnoreCase).Replace('*', 'X').Trim();
    }
}
