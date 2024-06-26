public struct RecipeMatch
{
    public RecipeMatch(
        bool isMatch,
        RecipeData data = null,
        Item itemA = null,
        Item itemB = null,
        ItemData itemC = default)
    {
        IsMatch = isMatch;
        Recipe = data;
        ItemA = itemA;
        ItemB = itemB;
        ItemC = itemC;
    }
    
    public bool IsMatch { get; }
    public RecipeData Recipe { get; }
    public Item ItemA { get; }
    public Item ItemB { get; }
    public ItemData ItemC { get; }
}