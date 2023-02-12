namespace DispelTools.DataEditor
{
    public abstract class MapperDefinition
    {
        public abstract int PropertyItemSize { get; }
        public virtual byte CounterSize { get; } = 4;
        public virtual string GetMapperName() => GetType().Name;
        public abstract List<ItemFieldDescriptor> CreateDescriptors();
    }
}
