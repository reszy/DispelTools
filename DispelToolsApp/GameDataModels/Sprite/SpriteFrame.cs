using DispelTools.Common;

namespace DispelTools.GameDataModels.Sprite
{
    public class SpriteFrame
    {
        public int OriginX { get; }
        public int OriginY { get; }
        public RawRgb RawRgb { get; }
        public SpriteFrame(int originX, int originY, RawRgb bitmap)
        {
            OriginX = originX;
            OriginY = originY;
            RawRgb = bitmap;
        }
    }
}
