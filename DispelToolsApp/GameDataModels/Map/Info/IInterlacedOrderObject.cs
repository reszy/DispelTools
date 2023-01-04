namespace DispelTools.GameDataModels.Map
{
    public interface IInterlacedOrderObject
    {
        int PositionOrder { get; }
        int Order { get; }
        int TypeOrder { get; }
    }
}
