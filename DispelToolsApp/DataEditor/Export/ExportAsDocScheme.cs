using DispelTools.Common;
using DispelTools.DataEditor.Data;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DispelTools.DataEditor.Export
{
    public class ExportAsDocScheme
    {
        private readonly static char SPACE = ' ';
        private readonly static char UNDERSCORE = '_';
        private readonly static string INDENTATION = new(SPACE, 4);

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

        private List<MapperDefinition> FindAllMappers()
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
            StringBuilder sb = new();
            int byteOffsetCounter = 0;
            sb.Append("struct ");
            sb.AppendLine(ConvertMapperNameToFilename(mapper.GetMapperName()));
            sb.AppendLine("{");
            if (mapper.CounterSize > 0)
            {
                AddVariableTypeAndName(sb, Field.DisplayType.HEX, mapper.CounterSize, "entries_count");
                sb.AppendLine();
            }
            AddComment(sb, "First entry scheme");
            foreach (var descriptor in mapper.CreateDescriptors())
            {
                if (descriptor.Name.StartsWith("?"))
                {
                    byteOffsetCounter += descriptor.ItemFieldDescriptorType.ByteSize;
                }
                else
                {
                    if (byteOffsetCounter > 0)
                    {
                        AddComment(sb, $"... Unknown {byteOffsetCounter} bytes");
                        byteOffsetCounter = 0;
                    }
                    AddFieldScheme(descriptor, sb);
                }
            }
            AddComment(sb, "... Remaining entries");
            sb.AppendLine("}");
            stringWriter.Write(sb);
        }

        private static void AddFieldScheme(ItemFieldDescriptor fieldDescriptor, StringBuilder sb)
        {
            var size = fieldDescriptor.ItemFieldDescriptorType.ByteSize;
            AddVariableTypeAndName(sb, fieldDescriptor.ItemFieldDescriptorType.VisualFieldType, size, fieldDescriptor.Name);
            var commentOffset = (int)Math.Ceiling(fieldDescriptor.Name.Length / 8.0);
            sb.Append(new string(SPACE, commentOffset * 8 - fieldDescriptor.Name.Length));
            sb.Append("  // ");
            var beforeBytes = sb.Length;
            sb.Append(size);
            sb.Append(size > 1 ? " bytes" : " byte");
            if (!string.IsNullOrEmpty(fieldDescriptor.Description))
            {
                sb.Append(new string(SPACE, 12 - (sb.Length - beforeBytes)));
                sb.Append(fieldDescriptor.Description);
            }
            sb.AppendLine();
        }

        private static void AddVariableTypeAndName(StringBuilder sb, Field.DisplayType type, int size, string name)
        {
            sb.Append(INDENTATION);
            var typeName = ConvertTypeName(type, size);
            sb.Append(typeName);
            sb.Append(new string(SPACE, 10 - typeName.Length));
            sb.Append(name.Replace(SPACE, UNDERSCORE));
        }

        private static void AddComment(StringBuilder sb, string comment)
        {
            sb.Append(INDENTATION);
            sb.AppendLine("//");
            sb.Append(INDENTATION);
            sb.Append("// ");
            sb.AppendLine(comment);
            sb.Append(INDENTATION);
            sb.AppendLine("//");
        }

        private static string ConvertTypeName(Field.DisplayType type, int size)
        {
            if (type == Field.DisplayType.HEX || type == Field.DisplayType.BIN || type == Field.DisplayType.DEC) {
                if (size == 1)
                {
                    return "BYTE";
                }
                else if (size == 2)
                {
                    return "INT16";
                }
                else if (size == 4)
                {
                    return "INT32";
                }
            }
            return $"BYTE[{size}]";
        }

        private static string ConvertMapperNameToFilename(string mapperName) => mapperName.Replace("Mapper", "", StringComparison.OrdinalIgnoreCase).Replace('*', 'X').Trim();
    }
}
