using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// Attached to the item Game Object. Controls the children. 
public class ItemRenderer : Touchable
{
    private readonly Dictionary<GameObject, Image> _renderers = new();
    private int _cacheSkipCount = 5;
    private GameObject _follow;
    private Vector3 _followPositionO;
    private Vector2 _followOffset;
    private SpriteData[] _sprites;
    private Item _item;
    
    public bool Destroyed { get; private set; }
    public object Initiator { get; set; }

    public event EventHandler OnDestroyed;

    public Item Item
    {
        get => _item;
        set => UpdateItem(value);
    }

    public event EventHandler Click;

    public override void Awake()
    {
        base.Awake();

        gameObject.layer = LayerMask.NameToLayer("UI");
    }

    public void Update()
    {
        FollowGameObject();
    }

    private void FollowGameObject()
    {
        if (Destroyed) return;
        if (!_follow.IsAssigned(() => _follow = null)) return;
        if (_cacheSkipCount > 0)
        {
            _cacheSkipCount--;
            _followPositionO = default;
        } 
        
        if (_followPositionO == _follow.transform.position) return;
        _followPositionO = _follow.transform.position;
        AlignTo(_follow, _followOffset);
    }

    public void AlignTo(GameObject value, Vector2 offset = default)
    {
        if (IsUIObject(value))
            AlignToUIObject(value, offset);
        else
            AlignTo3DObject(value, offset);
    }

    private bool IsUIObject(GameObject value)
    {
        // It's on UI layer => UI Object 
        if (value.layer == LayerMask.NameToLayer("UI")) return true;

        // It's a child of the overlay object -> Overlay = UI = 2D
        if (value.TryFindComponentInParents<Overlay>(out _)) return true;

        return false;
    }

    public void Follow(GameObject value, Vector2 offset = default)
    {
        _follow = value;
        _followOffset = offset;
        AlignTo(_follow, _followOffset);
    }

    public void StopFollowing()
    {
        _follow = null;
        _followOffset = default;
    }

    public void Refresh()
    {
        Item = Item;
    }

    protected override void OnTouch()
    {
        Click?.Invoke(this, EventArgs.Empty);
    }

    protected void AddSprite(SpriteData sprite)
    {
        if (sprite == null) return;
        var targetSize = sprite.Size == default ? sprite.Sprite.texture.Size() : sprite.Size;
        var rendererGameObject = Instantiate(GameSettings.Data.PRE_SpriteRenderer);
        var rendererComponent = rendererGameObject.GetRequiredComponent<Image>();
        rendererComponent.rectTransform.anchorMax = new Vector2(1, 1);
        rendererComponent.rectTransform.anchorMin = new Vector2(1, 1);
        rendererComponent.transform.SetParent(gameObject.transform);
        rendererComponent.sprite = sprite.Sprite;
        rendererComponent.rectTransform.sizeDelta = targetSize;
        rendererComponent.rectTransform.transform.localPosition = sprite.Offset;
        rendererComponent.rectTransform.transform.localScale = Vector2.one;
        rendererComponent.rectTransform.transform.localScale = Vector2.one;
        rendererComponent.rectTransform.transform.localRotation = Quaternion.Euler(0F, 0F, sprite.Rotation);
        rendererComponent.color = sprite.ColorOverlay;
        _renderers.Add(rendererGameObject, rendererComponent);
    }

    protected void RemoveSprite(SpriteData sprite)
    {
        var (rendererGameObject, _) = _renderers.First(x => x.Value.sprite == sprite.Sprite);
        _renderers.Remove(rendererGameObject);
        Destroy(rendererGameObject);
    }

    private void AlignToUIObject(GameObject value, Vector2 offset = default)
    {
        var offset3d = new Vector3(offset.x, offset.y, 0);
        var position = value.transform.position;
        gameObject.transform.position = position + offset3d;
    }

    private void AlignTo3DObject(GameObject value, Vector2 offset = default)
    {
        var offset3d = new Vector3(offset.x, offset.y, 0);
        var position = value.gameObject.transform.position;
        var screen = Raycaster.Instance.Get2DPositionFrom3D(position).ToVector3();
        gameObject.transform.position = screen + offset3d;
    }

    private void DetectChanges(List<SpriteData> newSprites, out SpriteData[] removed, out SpriteData[] added)
    {
        added = newSprites
            .Where(x => x is not null)
            .Where(x => !_sprites?.Contains(x) ?? true)
            .ToArray();

        removed = _sprites?
            .Where(x => !newSprites.Contains(x))
            .ToArray() ?? Array.Empty<SpriteData>();
    }

    private void UpdateItem(Item item)
    {
        _item = item ?? throw new ArgumentNullException(nameof(item));
        var sprites = _item.Data.Sprites.ToList();
        if (item.Data.Poison is not null) sprites.Add(GameSettings.Data.PoisonIcon);
        DetectChanges(sprites, out var removed, out var added);
        foreach (var removedSprite in removed) RemoveSprite(removedSprite);
        foreach (var addedSprite in added) AddSprite(addedSprite);
        IndexingSprites();
        gameObject.name = string.IsNullOrWhiteSpace(item.Data.Name) ? "ITEM | Unnamed Item" : $"ITEM | {item.Data.Name.Trim()}";
        _sprites = sprites.ToArray();
    }

    private void IndexingSprites()
    {
        if (_sprites is null) return;

        var spriteRenderers = _sprites
            .OrderBy(x => x.Order)
            .ToDictionary(x => x, x => _renderers.First(y => y.Value.sprite == x.Sprite).Value);

        for (var i = 0; i < spriteRenderers.Count; i++)
        {
            var image = spriteRenderers.ElementAt(i).Value;
            image.transform.SetSiblingIndex(i);
        }
    }

    public void Destroy()
    {
        if (Destroyed) return;
        Destroyed = true;
        OnDestroyed?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }

    public void SendToBack() => transform.SetAsFirstSibling();
    public void SendToFront() => transform.SetAsLastSibling();
}