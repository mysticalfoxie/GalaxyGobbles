using System;
using System.Collections.Generic;
using System.Linq;

public class CraftingSystem : SingletonMonoBehaviour<CraftingSystem>
{
    private RecipeData[] _recipes;

    public override void Awake()
    {
        base.Awake();

        _recipes = GameSettings.Data.Recipes.ToArray();
    }

    public IEnumerable<RecipeMatch> GetCraftables(IEnumerable<Item> itemList, Item item)
        => itemList
            .Select(x => MatchRecipe(x, item))
            .Where(x => x.IsMatch);

    public static Item CraftItem(RecipeData recipe, Item item1, Item item2)
    {
        if (recipe is null) 
            throw new ArgumentNullException(nameof(recipe));
        
        if (recipe.ItemA != item1.Data.Id && recipe.ItemB != item1.Data.Id) 
            throw new NotSupportedException($"{nameof(recipe.ItemA)} does not exist in the recipe with id {recipe.ItemC}.");
        
        if (recipe.ItemB != item2.Data.Id && recipe.ItemA != item2.Data.Id) 
            throw new NotSupportedException($"{nameof(recipe.ItemB)} does not exist in the recipe with id {recipe.ItemC}.");
        
        var itemData = GameSettings.Data.Items.First(x => x.Id == recipe.ItemC);
        var item = new Item(itemData, !item1.Hidden);
        if (item1.AlignedTo.IsAssigned()) item.AlignTo(item1.AlignedTo);
        if (item1.Following.IsAssigned()) item.Follow(item1.Following);
        item1.Dispose();
        item2.Dispose();
        return item;
    }

    private RecipeMatch MatchRecipe(Item item1, Item item2)
    {
        var recipe = _recipes.FirstOrDefault(x => x.ItemA == item1.Data.Id && x.ItemB == item2.Data.Id)
                     ?? _recipes.FirstOrDefault(x => x.ItemB == item1.Data.Id && x.ItemA == item2.Data.Id);

        return recipe is not null 
            ? new RecipeMatch(true, recipe, item1, item2, recipe.ItemC)
            : new RecipeMatch(false);
    }
}