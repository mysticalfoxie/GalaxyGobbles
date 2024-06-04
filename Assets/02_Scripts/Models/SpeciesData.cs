using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Species", menuName = "Galaxy Gobbles/Species", order = 3)]
public class SpeciesData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private ItemData[] _poisonItems;

    public string Name => _name;
    public Sprite Sprite => _sprite;
    public IEnumerable<ItemData> PoisonItems => _poisonItems;
}