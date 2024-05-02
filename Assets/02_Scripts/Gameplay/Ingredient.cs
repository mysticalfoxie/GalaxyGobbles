using System;
using System.Linq;

public class Ingredient : TouchableMonoBehaviour
{
    private IngredientData _data;
    private IngredientRenderer _renderer;
    
    public IngredientData Data
    {
        get => _data;
        protected set => UpdateData(value);
    }
    
    public IngredientType Type { get; private set; }

    public override void Awake()
    {
        base.Awake();
        
        _renderer = this.GetRequiredComponent<IngredientRenderer>();
    }

    private void UpdateData(IngredientData data)
    {
        _data = data ? data : throw new ArgumentNullException(nameof(data));
        Type = _data.Type;
        _renderer.Ingredient = _data;
    }

    public static Ingredient Create(IngredientType type)
    {
        var prefab = ReferencesSettings.Data.IngredientPrefab;
        var instance = Instantiate(prefab);
        instance.gameObject.SetActive(false);

        var ingredient = instance.GetRequiredComponent<Ingredient>();
        ingredient.Data = References.Instance.IngredientSettings.Ingredients.First(x => x.Type == type);
        ingredient.Type = type;

        return ingredient;
    }
}