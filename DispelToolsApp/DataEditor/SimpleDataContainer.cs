using DispelTools.DataEditor.Data;

namespace DispelTools.DataEditor
{
    public class SimpleDataContainer
    {
        private readonly List<DataItem> items;

        public MapperDefinition MapperDefinition { get; }
        public List<ItemFieldDescriptor> FieldDescriptors { get; }
        public DataItem this[int i] => items[i];
        public int Count => items.Count;
        public string Filename { get; }
        public string Path { get; }

        public SimpleDataContainer(MapperDefinition mapperDefinition, List<DataItem> items, string path, string filename)
        {
            this.items = new(items);
            MapperDefinition = mapperDefinition;
            FieldDescriptors = mapperDefinition.CreateDescriptors();
            Filename = filename;
            Path = path;
        }
        public int IndexOf(DataItem item) => items.IndexOf(item);
    }
}
