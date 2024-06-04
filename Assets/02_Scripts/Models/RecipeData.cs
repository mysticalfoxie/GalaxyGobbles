using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Galaxy Gobbles/Recipe", order = 5)]
public class RecipeData : ScriptableObject
{
    [Header("The item you want to craft")]
    [SerializeField] private ItemData _itemC;
    
    [Header("The combinations to craft it")]
    [SerializeField] private ItemData _itemA;
    [SerializeField] private ItemData _itemB;

    public void OnValidate()
    {
        if (_itemA is null) throw new Exception($"The recipe \"{name}\" does not have an entry for \"Item A\".");
        if (_itemB is null) throw new Exception($"The recipe \"{name}\" does not have an entry for \"Item B\".");
        if (_itemC is null) throw new Exception($"The recipe \"{name}\" does not have an entry for \"Item C\".");
    }

    public ItemData ItemA => _itemA;
    public ItemData ItemB => _itemB;
    public ItemData ItemC => _itemC;
}