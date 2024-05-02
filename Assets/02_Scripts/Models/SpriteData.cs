using UnityEngine;

public class SpriteData
{
    public SpriteData(Sprite sprite, Vector2? offset = null)
    {
        Sprite = sprite;
        Offset = offset;
    }

    public Sprite Sprite { get; }
    public Vector2? Offset { get; }
}