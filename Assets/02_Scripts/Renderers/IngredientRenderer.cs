public class IngredientRenderer : AbstractSpriteRenderer
{
    private IngredientData _ingredient;

    public IngredientData Ingredient
    {
        get => _ingredient;
        set => SetIngredient(value);
    }
    
    public void SetIngredient(IngredientData data)
    {
        if (_ingredient is not null) 
            RemoveSprite(_ingredient.Sprite);
        
        _ingredient = data;
        AddSprite(_ingredient.Sprite);
    }
}