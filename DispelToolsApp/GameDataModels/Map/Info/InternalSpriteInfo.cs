using System.Drawing;

namespace DispelTools.GameDataModels.Map
{
    public class InternalSpriteInfo
    {
        public InternalSpriteInfo(int id, Point position, Point bottomRightPosition)
        {
            Id = id;
            Position = position;
            BottomRightPosition = bottomRightPosition;
        }

        public int Id { get; set; }
        public Point Position { get; set; }
        public Point BottomRightPosition { get; set; }
    }
}
