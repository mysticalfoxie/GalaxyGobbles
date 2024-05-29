using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Galaxy Gobbles/Recipe", order = 5)]
public class RecipeData : ScriptableObject
{
    [Header("Recipe Data\nItem A + Item B = Item C")]
    
    [Header("Result (Item C)")]
    [SerializeField] private ItemData _itemC;
    
    [Header("Combination (Item A + B)")]
    [SerializeField] private ItemData _itemA;
    [SerializeField] private ItemData _itemB;

    public ItemData ItemA => _itemA;
    public ItemData ItemB => _itemB;
    public ItemData ItemC => _itemC;
}