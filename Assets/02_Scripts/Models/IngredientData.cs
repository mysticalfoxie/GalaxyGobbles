using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "GameData/Ingredient", order = 4)]
public class IngredientData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private IngredientType _type;
    [SerializeField] private SpriteData _sprite;
    
    public string Name => _name; 
    public IngredientType Type => _type; 
    public SpriteData Sprite => _sprite; 
}