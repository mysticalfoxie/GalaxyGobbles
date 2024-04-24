using System.Linq;
using UnityEngine;

public class TrashCan : TouchableMonoBehaviour
{
    private SpriteRenderer _renderer;

    [SerializeField] private Sprite _sprite;
    
    public override void Awake()
    {
        _renderer = this
            .GetChildren()
            .Select(x => x.GetComponent<SpriteRenderer>())
            .First(x => x is not null);
        
        _renderer.sprite = _sprite;
        
        base.Awake();
    }

    public override void OnClick()
    {
        Sidebar.Instance.Inventory.Reset();
    }
}