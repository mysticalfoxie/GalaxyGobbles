using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "GameData/Item", order = 4)]
public class ItemData : ScriptableObject
{
    [SerializeField]
    private string _name;

    [SerializeField] 
    private ItemCategory _category;

    [SerializeField] 
    private Sprite _sprite;
    
    [SerializeField] 
    private bool _deliverable;

    public string Name => _name;
    public ItemCategory Category => _category;
    public Sprite Sprite => _sprite;
    public bool Deliverable => _deliverable;
}