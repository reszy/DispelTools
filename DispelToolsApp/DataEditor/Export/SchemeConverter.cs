using DispelTools.DataEditor.Data;
using System.IO.Abstractions;
using System.Text;

namespace DispelTools.DataEditor.Export
{
    internal class SchemeConverter
    {
        private readonly MapperDefinition mapperDefinition;
        public SchemeConverter(MapperDefinition mapperDefinition)
        {
            this.mapperDefinition = mapperDefinition;
        }
        public StringBuilder ToTxt(string name)
        {
            StringBuilder stringBuilder = new();
            TxtConverter.CreateScheme(stringBuilder, mapperDefinition, name);
            return stringBuilder;
        }

        private class TxtConverter
        {
            private readonly static char SPACE = ' ';
            private readonly static char UNDERSCORE = '_';
            private readonly static string INDENTATION = new(SPACE, 4);

            public static void CreateScheme(StringBuilder stringBuilder, MapperDefinition mapper, string name)
            {
                int byteOffsetCounter = 0;
                stringBuilder.Append("struct ");
                stringBuilder.AppendLine(name);
                stringBuilder.AppendLine("{");
                if (mapper.InFileCounterSize > 0)
                {
                    AddVariableTypeAndName(stringBuilder, Field.DisplayType.HEX, mapper.InFileCounterSize, "entries_count");
                    stringBuilder.AppendLine();
                }
                AddComment(stringBuilder, "First entry scheme");
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
                            AddComment(stringBuilder, $"... Unknown {byteOffsetCounter} bytes");
                            byteOffsetCounter = 0;
                        }
                        AddFieldScheme(descriptor, stringBuilder);
                    }
                }
                AddComment(stringBuilder, "... Remaining entries");
                stringBuilder.AppendLine("}");
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
                if (type == Field.DisplayType.HEX || type == Field.DisplayType.BIN || type == Field.DisplayType.DEC)
                {
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
        }
    }
}
