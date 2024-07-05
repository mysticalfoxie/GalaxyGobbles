using UnityEngine;

public class TrashCan : Touchable
{
    [Header("Item Visualization")]
    [SerializeField] private RectTransform _canvas;
    [SerializeField] [Range(0.1F, 5.0F)] private float _scale = 1;

    public override void Awake()
    {        
        base.Awake();

        new Item(new(this, GameSettings.GetItemMatch(Identifiers.Value.Trash), true, ItemDisplayDimension.Dimension3D))
            .ForwardTouchEventsTo(this)
            .SetParent(_canvas.transform)
            .SetLocalPosition(Vector2.zero)
            .SetRotation(Vector2.zero)
            .SetScale(new Vector2(_scale, _scale));
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