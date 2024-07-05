public struct ItemCreationContext
{
    public ItemCreationContext(
        object initiator, 
        ItemData data, 
        bool showAfterCreation = false, 
        ItemDisplayDimension dimension = ItemDisplayDimension.Dimension2D)
    {
        Initiator = initiator;
        Data = data;
        ShowAfterCreation = showAfterCreation;
        Dimension = dimension;
    }
    
    public object Initiator { get; }
    public ItemData Data { get; }
    public bool ShowAfterCreation { get; }
    public ItemDisplayDimension Dimension { get; }
}