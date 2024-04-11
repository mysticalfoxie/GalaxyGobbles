using System.Linq;
using UnityEngine;

public class ItemProvider : TouchableMonoBehaviour
{
    private SpriteRenderer _renderer;
    
    [Header("Provider Data")]
    [SerializeField]
    private ItemData _item;

    public override void Awake()
    {
        _renderer = this
            .GetChildren()
            .Select(x => x.GetComponent<SpriteRenderer>())
            .First(x => x is not null);
        
        _renderer.sprite = _item.Sprite;
        
        base.Awake();
    }

    public override void OnClick()
    { 
        Inventory.Instance.Add(_item);
    }
}