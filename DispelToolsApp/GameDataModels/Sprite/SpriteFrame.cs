using DispelTools.Common;

namespace DispelTools.GameDataModels.Sprite
{
    public class SpriteFrame
    {
        public int OriginX { get; }
        public int OriginY { get; }
        public DirectBitmap Bitmap { get; }
        public SpriteFrame(int originX, int originY, DirectBitmap bitmap)
        {
            OriginX = originX;
            OriginY = originY;
            Bitmap = bitmap;
        }
    }
}
