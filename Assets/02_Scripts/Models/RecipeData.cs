using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Galaxy Gobbles/Recipe", order = 5)]
public class RecipeData : ScriptableObject
{
    [Header("Recipe Data\nItem A + Item B = Item C")]
    
    [Header("Result (Item C)")]
    [SerializeField] private ItemId _itemC;
    
    [Header("Combination (Item A + B)")]
    [SerializeField] private ItemId _itemA;
    [SerializeField] private ItemId _itemB;

    public ItemId ItemA => _itemA;
    public ItemId ItemB => _itemB;
    public ItemId ItemC => _itemC;
}