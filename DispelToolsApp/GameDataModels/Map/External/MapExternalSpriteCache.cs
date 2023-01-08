using DispelTools.GameDataModels.Sprite;
using DispelTools.ImageProcessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispelTools.GameDataModels.Map.External
{
    class MapExternalSpriteCache
    {
        private readonly string spriteDirectory;
        private readonly Dictionary<int, string> nameCache;
        private readonly Dictionary<int, CachedDirectionalSprite> spriteCache;

        public MapExternalSpriteCache(string spriteDirectory, Dictionary<int, string> nameCache)
        {
            spriteCache = new Dictionary<int, CachedDirectionalSprite>();
            this.spriteDirectory = spriteDirectory;
            this.nameCache = nameCache;
        }

        public SpriteFrame GetSpriteFrame(int spriteId, int spriteSequence)
        {
            var haveValue = spriteCache.TryGetValue(spriteId, out var cached);
            if (!haveValue)
            {
                var filename = nameCache[spriteId];
                using (var file = new BinaryReader(File.OpenRead($"{spriteDirectory}\\{filename}")))
                {
                    var processor = new SpriteProcessor(file, filename);
                    SpriteFileReader.ProcessThroughFile(processor, false);
                    spriteCache[spriteId] = cached = new CachedDirectionalSprite(processor.Sequences.ToArray());
                }
            }

            return cached[spriteSequence];
        }

        private class CachedDirectionalSprite
        {
            public SpriteFrame[] sprites;

            public CachedDirectionalSprite()
            {
                sprites = new SpriteFrame[7];
            }

            public CachedDirectionalSprite(SpriteFrame[] sprites)
            {
                this.sprites = sprites;
            }

            public SpriteFrame this[int id] => id < sprites.Length ? sprites[id] : sprites[0];
        }

        private class SpriteProcessor : OpenedSpriteFile
        {
            public SpriteProcessor(BinaryReader file, string filename) : base(file, filename, ColorManagement.ColorMode.RGB16_565)
            {
            }

            public List<SpriteFrame> Sequences { get; } = new List<SpriteFrame>();

            public override void Process(SpriteSequence sequence, int imageNumber)
            {
                Sequences.Add(sequence.GetFrame(0));
            }
        }
    }
}
