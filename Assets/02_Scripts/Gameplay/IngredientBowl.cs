using System.Linq;
using UnityEngine;

public class IngredientBowl : SpriteRendererBase
{
    [SerializeField] 
    private IngredientType _ingredientType;
    private IngredientData _ingredientData;

    public override void Awake()
    {
        base.Awake();
        
        _ingredientData = References.Instance.Ingredients.All.First(x => x.Type == _ingredientType);
        RenderSprite(_ingredientData.Sprite);
    }
}