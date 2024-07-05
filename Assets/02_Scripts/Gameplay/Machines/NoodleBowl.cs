using UnityEngine;

public class NoodleBowl : Touchable
{
    private Item _item;
    private ItemData _itemData;

    [Header("Item Visualization")] 
    [SerializeField] private RectTransform _canvas;
    [SerializeField] [Range(0.1F, 5.0F)] private float _scale = 1;

    public override void Awake()
    {
        base.Awake();

        _itemData = GameSettings.GetItemMatch(Identifiers.Value.NoodleBowl);
        _item = new Item(new(this, _itemData, true, ItemDisplayDimension.Dimension3D))
            .ForwardTouchEventsTo(this)
            .SetParent(_canvas.transform)
            .SetLocalPosition(Vector2.zero)
            .SetRotation(Vector2.zero)
            .SetScale(new Vector2(_scale, _scale));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.HSVToRGB(.1F, .7F, .7F);
        Gizmos.DrawCube(transform.position, transform.lossyScale);
    }

    protected override void OnTouch()
    {
        NoodlePotDistributor.AddNoodles();
    }
}