using DispelTools.DataEditor.Data;

namespace DispelTools.DataEditor
{
    public class FieldMapping
    {
        private readonly int[] mapping;

        public FieldMapping(MapperDefinition definition, params string[] fieldNames)
        {
            var descriptorList = definition.CreateDescriptors();
            mapping = fieldNames
                .Select(name => descriptorList.Find(descriptor => descriptor.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                .Select(descriptor => descriptorList.IndexOf(descriptor))
                .ToArray();
        }

        public object[] Convert(DataItem item)
        {
            return mapping
                .Select(fieldId => item[fieldId].Value)
                .ToArray();
        }
    }
}