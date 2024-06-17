using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Species", menuName = "Galaxy Gobbles/Species", order = 3)]
public class SpeciesData : ScriptableObject
{
    [Tooltip("Used for dialogs and all human readable contexts.")]
    [SerializeField] private string _name;
    [SerializeField] private Sprite _sprite;
    [Tooltip("To what is this species poisoned?")]
    [SerializeField] private ItemData[] _poisonItems;
    
    [Header("Behaviours")]
    [Tooltip("The chance for the customer to just leave without ordering.")]
    [Range(0, 100)] [SerializeField] private float _chanceToNotOrder;
    [Tooltip("The chance for the customer to order twice.")]
    [Range(0, 100)] [SerializeField] private float _chanceToOrderTwice;
    [Tooltip("The chance for the customer to order sake.")]
    [Range(0, 100)] [SerializeField] private float _chanceToOrderSake;

    public string Name => _name;
    public Sprite Sprite => _sprite;
    public IEnumerable<ItemData> PoisonItems => _poisonItems;
    public float ChanceToNotOrder => _chanceToNotOrder;
    public float ChanceToOrderSake => _chanceToOrderSake;
    public float ChanceToOrderTwice => _chanceToOrderTwice;
}