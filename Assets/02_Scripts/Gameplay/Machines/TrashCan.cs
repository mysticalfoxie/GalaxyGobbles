using UnityEngine;

public class TrashCan : Touchable
{
    [Header("Item Visualization")]
    [SerializeField] private Vector2 _offset;
    
    private Item _item;

    public override void Awake()
    {        
        base.Awake();

        _item = new Item(this, GameSettings.GetItemMatch(Identifiers.Value.Trash), true);
        _item.AlignTo(this, _offset);
        _item.ForwardTouchEventsTo(this);
    }

    private void OnValidate()
    {
        GameSettings.GetItemMatch(Identifiers.Value.Trash);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.HSVToRGB(0F, .7F, .7F);
        Gizmos.DrawCube(transform.position, transform.lossyScale);
    }

    protected override void OnTouch()
    {
        BottomBar.Instance.Inventory.Reset();
    }
}