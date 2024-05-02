using System;
using System.Linq;
using UnityEngine;

public class Ingredient : SpriteRendererBase
{
    private Sprite _sprite;
    private IngredientData _data;
    
    public IngredientData Data
    {
        get => _data;
        protected set => UpdateData(value);
    }

    private void UpdateData(IngredientData data)
    {
        _data = data ? data : throw new ArgumentNullException(nameof(data));
        RenderSprite(data);
    }
    
    
    
    
    
    
    
    
    
    
    public IngredientType Type { get; private set; }

    public static Ingredient Create(IngredientType type)
    {
        var prefab = ReferencesSettings.Data.IngredientPrefab;
        var instance = Instantiate(prefab);
        instance.gameObject.SetActive(false);

        var ingredient = instance.GetRequiredComponent<Ingredient>();
        ingredient.Data = References.Instance.Ingredients.All.First(x => x.Type == type);
        ingredient.Type = type;

        return ingredient;
    }
}