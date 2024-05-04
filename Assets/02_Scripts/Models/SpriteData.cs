using UnityEngine;

public class SpriteData
{
    public SpriteData(Sprite sprite, int order = 0, Vector2? offset = null)
    {
        Sprite = sprite;
        Offset = offset;
        Order = order;
    }

    public Sprite Sprite { get; }
    public Vector2? Offset { get; }
    public int Order { get; }
}