using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Galaxy Gobbles/Item", order = 4)]
public class ItemData : ScriptableObject
{
    [Header("Game Data")]
    [SerializeField] private ItemId _id;
    [SerializeField] private string _name;
    [SerializeField] private SpriteData[] _sprites;

    public string Name => _name;
    public ItemId Id => _id;
    public IEnumerable<SpriteData> Sprites => _sprites;
}