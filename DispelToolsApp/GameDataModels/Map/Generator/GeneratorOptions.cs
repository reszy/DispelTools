namespace DispelTools.GameDataModels.Map.Generator
{
    public class GeneratorOptions
    {
        public bool Occlusion { get; set; } = true;
        public bool GTL { get; set; } = true;
        public bool Sprites { get; set; } = false;
        public bool Collisions { get; set; } = false;
        public bool TiledObjects { get; set; } = false;
        public bool Roofs { get; set; } = false;
        public bool Events { get; set; } = false;
    }
}
