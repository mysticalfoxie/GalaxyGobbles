using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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

    [Header("Positioning")] 
    [Tooltip("The size of the collider for the species.")]
    [SerializeField] private Vector3 _colliderSize;
    [Tooltip("The offset the customer has to a chair. (The chair used in the horizontal table prefab)\nThe x value becomes inverted for chairs that are positioned on the right. So keep in mind that your offset should be from the left chair.")]
    [SerializeField] private Vector3 _chairOffsetHorizontal;
    [Tooltip("The offset the customer has to a chair. (The chair used in the vertical table prefab)\nThe x value becomes inverted for chairs that are positioned on the right. So keep in mind that your offset should be from the left chair.")]
    [SerializeField] private Vector3 _chairOffsetVertical;
    [Tooltip("The offset of the hearts to the customer. This needs to be configured to the left side, because it's flipped later on when you seat your customer on a right seat.")]
    [SerializeField] private Vector2 _heartsOffset;
    [Tooltip("The offset of the thinking bubble in relation to the customer. This needs to be configured to the right side, because it's flipped later on.")]
    [SerializeField] private Vector2 _thinkBubbleOffset;
    [FormerlySerializedAs("_thinkBubbleOffsetHorizontal")]
    [FormerlySerializedAs("_tableThinkBubbleOffsetHorizontal")]
    [Tooltip("The offset of the thinking bubble at the horizontal table in relation to the customer. This needs to be configured to the right side, because it's flipped later on.")]
    [SerializeField] private Vector2 _mealsThinkBubbleOffsetHorizontal;
    [FormerlySerializedAs("_tableThinkBubbleOffsetVertical")]
    [Tooltip("The offset of the thinking bubble at the vertical table in relation to the customer. This needs to be configured to the right side, because it's flipped later on.")]
    [SerializeField] private Vector2 _mealsThinkBubbleOffsetVertical;

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
    public Vector3 ColliderSize => _colliderSize;
    public Vector3 ChairOffsetHorizontal => _chairOffsetHorizontal;
    public Vector3 ChairOffsetVertical => _chairOffsetVertical;
    public Vector2 HeartsOffset => _heartsOffset;
    public Vector2 ThinkBubbleOffset => _thinkBubbleOffset;
    public Vector2 MealsThinkBubbleOffsetHorizontal => _mealsThinkBubbleOffsetHorizontal;
    public Vector2 MealsThinkBubbleOffsetVertical => _mealsThinkBubbleOffsetVertical;
}