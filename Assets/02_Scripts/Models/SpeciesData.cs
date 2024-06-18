using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Species", menuName = "Galaxy Gobbles/Species", order = 3)]
public class SpeciesData : ScriptableObject
{
    [Tooltip("Used for dialogs and all human readable contexts.")]
    [SerializeField] private string _name;
    [Tooltip("1.0F = normal [anchor], e.g.: 1.3 = 130% scale relative to the anchor.")]
    [SerializeField] [Range(0, 2F)] private float _scale = 1.0F;
    [SerializeField] private Sprite _frontSprite;
    [SerializeField] private Sprite _backSprite;
    [SerializeField] private Sprite _sideSprite;
    [Tooltip("The character is facing to the left side.")]
    [SerializeField] private Sprite _sittingSprite;
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
    public float Scale => _scale;
    public Sprite FrontSprite => _frontSprite;
    public Sprite SideSprite => _sideSprite;
    public Sprite BackSprite => _backSprite;
    public Sprite SittingSprite => _sittingSprite;
    public IEnumerable<ItemData> PoisonItems => _poisonItems;
    public float ChanceToNotOrder => _chanceToNotOrder;
    public float ChanceToOrderSake => _chanceToOrderSake;
    public float ChanceToOrderTwice => _chanceToOrderTwice;
}