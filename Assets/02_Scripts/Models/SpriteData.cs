using UnityEngine;

[CreateAssetMenu(fileName = "Sprite", menuName = "Galaxy Gobbles/Sprite", order = 5)]
public class SpriteData : ScriptableObject
{
    public SpriteData() { }
    public SpriteData(Sprite sprite, int order = 0, Vector2? size = null, Vector2? offset = null, float? rotation = null)
    { 
        _sprite = sprite;
        _offset = offset ?? _offset;
        _size = size ?? _size;
        _zRotation = rotation ?? _zRotation;
        _zOrder = order;
    }

    [Header("Sprite Data")] 
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Vector2 _offset;
    [SerializeField] private Vector2 _size;
    [SerializeField] private Color _colorOverlay = Color.white;
    [SerializeField] private float _zRotation;
    [SerializeField] private int _zOrder;

    public Sprite Sprite => _sprite;
    public Vector2 Offset => _offset;
    public Vector2 Size => _size;
    public Color ColorOverlay => _colorOverlay;
    public float Rotation => _zRotation;
    public int Order => _zOrder;
}