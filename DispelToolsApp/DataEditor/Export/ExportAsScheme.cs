using DispelTools.Common.DataProcessing;
using System.IO.Abstractions;

namespace DispelTools.DataEditor.Export
{
    public class ExportAsScheme : Exporter
    {
        private readonly MapperDefinition mapperDefinition;
        public ExportAsScheme(IFileSystem fs, WorkReporter workReporter, SimpleDataContainer container) : base(fs, workReporter)
        {
            mapperDefinition = container.MapperDefinition;
        }

        public override void Export(string path)
        {
            workReporter.SetTotal(100);
            using var file = fs.File.Create(path);
            using var stringWriter = new StreamWriter(file);
            SchemeConverter schemeConverter = new(mapperDefinition);
            var stringBuilder = schemeConverter.ToTxt(fs.Path.GetFileNameWithoutExtension(path));
            stringWriter.Write(stringBuilder);
            workReporter.ReportProgress(100);
        }

        private static string ConvertMapperNameToFilename(string mapperName) => mapperName.Replace("Mapper", "", StringComparison.OrdinalIgnoreCase).Replace('*', 'X').Trim();

        public static string GetDefaultFilename(SimpleDataContainer container) => ConvertMapperNameToFilename(container.MapperDefinition.GetMapperName());
    }
}
