using System.Linq;
using UnityEngine;

public class SpriteRendererBase : TouchableMonoBehaviour
{
    public SpriteRenderer Sprite { get; private set; }
    
    public override void Awake()
    { 
        Sprite = this
            .GetChildren()
            .Select(x => x.GetComponent<SpriteRenderer>())
            .First(x => x is not null);
        
        base.Awake();
    }

    protected void RenderSprite(Sprite sprite) => Sprite.sprite = sprite;
    protected void RenderSprite(ItemData item) => Sprite.sprite = item.Sprite;
    protected void RenderSprite(IngredientData ingredient) => Sprite.sprite = ingredient.Sprite;
    protected void RenderSprite(Item item) => Sprite.sprite = item.Data.Sprite;
}