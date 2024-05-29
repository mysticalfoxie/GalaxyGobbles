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
        
        if (recipe.ItemA.name != item1.Data.name && recipe.ItemB.name != item1.Data.name) 
            throw new NotSupportedException($"Item \"{nameof(recipe.ItemA.name)}\" does not exist in the recipe for item \"{recipe.ItemC.name}\".");
        
        if (recipe.ItemB.name != item2.Data.name && recipe.ItemA.name != item2.Data.name) 
            throw new NotSupportedException($"Item \"{nameof(recipe.ItemB.name)}\" does not exist in the recipe for item \"{recipe.ItemC.name}\".");
        
        var itemData = GameSettings.GetItemMatch(recipe.ItemC);
        var item = new Item(item1.Initiator, itemData, !item1.Hidden);
        if (item1.AlignedTo.IsAssigned()) item.AlignTo(item1.AlignedTo);
        if (item1.Following.IsAssigned()) item.Follow(item1.Following);
        item1.Dispose();
        item2.Dispose();
        return item;
    }

    private RecipeMatch MatchRecipe(Item item1, Item item2)
    {
        var recipe = _recipes.FirstOrDefault(x => x.ItemA.name == item1.Data.name && x.ItemB.name == item2.Data.name)
                     ?? _recipes.FirstOrDefault(x => x.ItemB.name == item1.Data.name && x.ItemA.name == item2.Data.name);

        return recipe is not null 
            ? new RecipeMatch(true, recipe, item1, item2, recipe.ItemC)
            : new RecipeMatch(false);
    }
}