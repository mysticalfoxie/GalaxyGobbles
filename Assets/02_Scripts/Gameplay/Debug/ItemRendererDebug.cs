using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class ItemRendererDebug : MonoBehaviour
{
    private SpriteData[] _sprites;

    [SerializeField] private ItemData _itemData;
    [SerializeField] private bool _poisoned;

    public void Update()
    {
        UpdateItem(_itemData);
    }

    protected void AddSprite(SpriteData sprite)
    {
        if (!sprite || !sprite.Sprite) return;
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
    }

    protected void RemoveSprites()
    {
        var array = this.GetChildren().ToArray();
        foreach (var child in array) 
            DestroyImmediate(child);
    }

    private void UpdateItem(ItemData item)
    {
        RemoveSprites();
        var sprites = _itemData.Sprites.ToList();
        if (_poisoned) sprites.Add(GameSettings.Data.PoisonIcon);
        foreach (var sprite in sprites) AddSprite(sprite);
        gameObject.name = string.IsNullOrWhiteSpace(item.Name) ? "ITEM | Unnamed Item" : $"ITEM | {item.Name.Trim()}";
        _sprites = sprites.ToArray();
    }

}