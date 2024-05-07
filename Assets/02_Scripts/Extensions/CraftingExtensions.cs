using System;
using System.Collections.Generic;

public static class CraftingExtensions
{
    public static IEnumerable<RecipeMatch> GetCraftableRecipesWith(this IEnumerable<Item> itemList, Item item2)
        => CraftingSystem.Instance.GetCraftables(itemList, item2);

    public static Item Fulfill(this RecipeMatch match)
    {
        if (!match.IsMatch) throw new NotSupportedException("Cannot craft a recipe that is not fulfilled.");
        return match.Recipe.Fulfill(match.ItemA, match.ItemB);
    }
    
    public static Item Fulfill(this RecipeData recipe, Item item1, Item item2)
        => CraftingSystem.CraftItem(recipe, item1, item2);
}