using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Galaxy Gobbles/Item", order = 4)]
public class ItemData : ScriptableObject
{
    [Header("Game Data")] 
    [Tooltip("You can add \"{FILENAME}\" to use the filename, but it's not recommended.\nPlease customize the name, especially when it's mentioned by the kitty bot.")]
    [SerializeField] private string _name;
    [Tooltip("If the item is deliverable to a customer.")]
    [SerializeField] private bool _deliverable;
    [Tooltip("If the item is able to be cut in the cutting board to become poison.")]
    [SerializeField] private bool _canBecomePoison;
    [Tooltip("If the item is able to be combined with any sort of poison.")]
    [SerializeField] private bool _canBecomePoisoned;
    [Tooltip("The base score you receive from this food.")]
    [SerializeField] private float _score = 1.0F;
    [Tooltip("The sprites required to render this item.")]
    [SerializeField] private SpriteData[] _sprites;

    public string Name => _name;
    public IEnumerable<SpriteData> Sprites => _sprites;
    public bool Deliverable => _deliverable;
    public bool CanBecomePoison => _canBecomePoison;
    public bool CanBecomePoisoned => _canBecomePoisoned;
    public float Score => _score;
    public ItemData Poison { get; set; }

    private void OnValidate()
    {
        _name = _name.Replace("{FILENAME}", name);
    }
}