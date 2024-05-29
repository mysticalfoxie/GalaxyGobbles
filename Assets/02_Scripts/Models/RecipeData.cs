using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Galaxy Gobbles/Recipe", order = 5)]
public class RecipeData : ScriptableObject
{
    [Header("Recipe Data\nItem A + Item B = Item C")]
    
    [Header("Result (Item C)")]
    [SerializeField] private string _itemC;
    
    [Header("Combination (Item A + B)")]
    [SerializeField] private string _itemA;
    [SerializeField] private string _itemB;

    public string ItemA => _itemA;
    public string ItemB => _itemB;
    public string ItemC => _itemC;
}