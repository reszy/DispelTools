using System.Drawing;

namespace DispelTools.GameDataModels.Map
{
    public class InternalSpriteInfo : IInterlacedOrderObject
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

        int IInterlacedOrderObject.PositionOrder => BottomRightPosition.Y;

        int IInterlacedOrderObject.Order => 0;

        int IInterlacedOrderObject.TypeOrder => -1;
    }
}
