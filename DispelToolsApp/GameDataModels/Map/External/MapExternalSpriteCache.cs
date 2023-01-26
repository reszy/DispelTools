using DispelTools.Common;
using DispelTools.GameDataModels.Sprite;
using DispelTools.ImageProcessing;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DispelTools.GameDataModels.Map.External
{
    class MapExternalSpriteCache
    {
        private readonly string spriteDirectory;
        private readonly Dictionary<int, string> nameCache;
        private readonly Dictionary<int, CachedDirectionalSprite> spriteCache;

        private static SpriteFrame Unknown { get => unknown ?? CreateUnknownSprite();  }

        private static SpriteFrame unknown;
        private static SpriteFrame CreateUnknownSprite()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DispelTools.Resources.unknown.png"))
            {
                var image = new MagickImage(stream);
                var rawRgbImage = new RawRgb(image.Width, image.Height);
                using (var memory = new MemoryStream(rawRgbImage.Bytes, true))
                    image.Write(memory, MagickFormat.Rgb);
                unknown = new SpriteFrame(10, 58, rawRgbImage);
                return unknown;
            }
        }

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
                if (!nameCache.ContainsKey(spriteId)) return Unknown;

                var filename = nameCache[spriteId];
                var filePath = $"{spriteDirectory}\\{filename}";
                if (File.Exists(filePath))
                {
                    using (var file = new BinaryReader(File.OpenRead(filePath)))
                    {
                        var processor = new SpriteProcessor(file, filename);
                        SpriteFileReader.ProcessThroughFile(processor, false);
                        spriteCache[spriteId] = cached = new CachedDirectionalSprite(processor.Sequences.ToArray());
                    }
                }
                else
                {
                    return Unknown;
                }
            }

            return cached[spriteSequence];
        }

        public string GetSpriteName(int spriteId) => nameCache.ContainsKey(spriteId) ? nameCache[spriteId] : "NotFound";

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
