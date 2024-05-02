using System.Linq;
using UnityEngine;

public class IngredientProvider : TouchableMonoBehaviour
{
    [SerializeField] 
    private IngredientType _ingredientType;
    private IngredientData _ingredientData;
    private IngredientRenderer _renderer;

    public override void Awake()
    {
        base.Awake();
        
        _ingredientData = References.Instance.Ingredients.All.First(x => x.Type == _ingredientType);
        _renderer = this.GetRequiredComponent<IngredientRenderer>();
        _renderer.SetIngredient(_ingredientData);
    }
}