using System;
using UnityEngine;
 
public class Item : IDisposable
{
    private readonly ItemCreationContext _context;
    private ItemRenderer _renderer;
    private readonly bool _initialized;

    public Item(ItemCreationContext context)
    {
        _context = context;
        Id = Guid.NewGuid();
        Dimension = context.Dimension;
        Initiator = context.Initiator;
        Data = context.Data;
        if (context.ShowAfterCreation) Show();
        else Hidden = true;
        _initialized = true;
    }

    public Guid Id { get; }
    public object Initiator { get; }
    public ItemDisplayDimension Dimension { get; }
    public ItemData Data { get; }
    public GameObject Following { get; private set; }
    public Vector2 FollowOffset { get; private set; }
    public bool Hidden { get; private set; }
    public Transform Parent { get; private set; }
    public Vector2? LocalPosition { get; private set; }
    public Vector3? Rotation { get; private set; }
    public Vector3? Scale { get; set; }
    public event EventHandler Click;

    public Item Show()
    {
        if (!Hidden && _initialized) return this;
        _renderer = CreateItemRenderer();
        if (Following) _renderer.Follow(Following, FollowOffset);
        Hidden = false;
        return this;
    }

    public Item Hide()
    {
        if (Hidden) return this;
        _renderer.Destroy();
        Hidden = true;
        return this;
    }
    
    public Item Follow(Item value, Vector2 offset = default) => Follow(value._renderer?.gameObject, offset);
    public Item Follow(MonoBehaviour value, Vector2 offset = default) => Follow(value.gameObject, offset);
    public Item Follow(GameObject value, Vector2 offset = default)
    {
        if (!value) return this; 
        Following = value;
        FollowOffset = offset;
        if (!_renderer) return this;
        _renderer.AlignTo(value, offset);
        _renderer.Follow(value, offset);
        return this;
    }

    public Item SetParent(Transform parent)
    {
        Parent = parent;
        if (_renderer) _renderer.SetParent(parent);
        return this;
    }

    public Item SetLocalPosition(Vector2 position)
    {
        LocalPosition = position;
        if (_renderer) _renderer.UpdatePosition();
        return this;
    }

    public Item SetScale(Vector3 scale)
    {
        Scale = scale;
        if (_renderer) _renderer.UpdateScale();
        return this;
    }
    
    public Item SetRotation(Vector3 rotation)
    {
        Rotation = rotation;
        if (_renderer) _renderer.UpdateRotation();
        return this;
    }

    public Item StopFollowing()
    {
        _renderer.StopFollowing();
        return this;
    }

    public Item SendToBack()
    {
        _renderer.SendToBack();
        return this;
    }

    public Item SendToFront()
    {
        _renderer.SendToFront();
        return this;
    }

    public void Refresh()
    {
        _renderer.Refresh();
    }

    public Item ForwardTouchEventsTo(Touchable touchable)
    {
        Click += (_, _) => touchable?.InvokeTouch(this, EventArgs.Empty);
        return this;
    }

    public Item Clone()
    {
        var clone = new ItemCreationContext(this, Data.Clone(), _context.ShowAfterCreation, _context.Dimension);
        return new Item(clone);
    }

    public void Dispose()
    {
        if (_renderer is not null && !_renderer.Destroyed) 
            _renderer.Destroy();
        
        GC.SuppressFinalize(this);
    }

    private ItemRenderer CreateItemRenderer()
    {
        var itemRenderer = Overlay.Instance.CreateItemRenderer(this, Initiator);
        itemRenderer.Item = this;
        itemRenderer.Click += (o, e) => Click?.Invoke(o, e);
        return itemRenderer;
    }
}