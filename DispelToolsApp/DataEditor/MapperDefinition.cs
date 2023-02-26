namespace DispelTools.DataEditor
{
    public abstract class MapperDefinition
    {
        public abstract int ItemSize { get; }
        public virtual byte InFileCounterSize { get; } = 4;
        public virtual string GetMapperName() => GetType().Name;
        public abstract List<ItemFieldDescriptor> CreateDescriptors();
    }
}
