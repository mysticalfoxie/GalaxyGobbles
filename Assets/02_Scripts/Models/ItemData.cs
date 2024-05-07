using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Galaxy Gobbles/Item", order = 4)]
public class ItemData : ScriptableObject
{
    [Header("Game Data")]
    [SerializeField] private ItemId _id;
    [SerializeField] private string _name;
    [SerializeField] private bool _deliverable;
    [SerializeField] private SpriteData[] _sprites;

    public string Name => _name;
    public ItemId Id => _id;
    public IEnumerable<SpriteData> Sprites => _sprites;
    public bool Deliverable => _deliverable;

    public ItemData Clone() 
        => new()
        {
            _id = Id,
            _name = Name,
            _deliverable = Deliverable,
            _sprites = Sprites.ToArray(),
        };
}