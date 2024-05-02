using System.Collections.Generic;

public class Noodles : Item
{
    private readonly List<Ingredient> _ingredients = new();
    
    private bool _boiled;
    private bool _overcooked;

    public void AddIngredient(IngredientType type)
    {
        var ingredient = Ingredient.Create(type);
        _ingredients.Add(ingredient);
    }

    public static Noodles Create()
    {
        var prefab = ReferencesSettings.Data.NoodlesPrefab;
        var instance = Instantiate(prefab);
        instance.gameObject.SetActive(false);

        var item = instance.GetRequiredComponent<Noodles>();
        item.Data = References.Instance.Items.Noodles;

        return item;
    }
}