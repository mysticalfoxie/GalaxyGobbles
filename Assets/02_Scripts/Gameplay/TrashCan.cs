using System.Linq;
using UnityEngine;

public class TrashCan : TouchableMonoBehaviour
{
    private UnityEngine.SpriteRenderer _renderer;

    [SerializeField] private Sprite _sprite;
    
    public override void Awake()
    {
        _renderer = this
            .GetChildren()
            .Select(x => x.GetComponent<UnityEngine.SpriteRenderer>())
            .First(x => x is not null);
        
        _renderer.sprite = _sprite;
        
        base.Awake();
    }

    protected override void OnTouch()
    {
        Sidebar.Instance.Inventory.Reset();
    }
}