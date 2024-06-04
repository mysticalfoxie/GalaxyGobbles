using UnityEngine;

public class RectTransformWrapper : MonoBehaviour
{
    private Vector2 _position;
    private RectTransform _rectTransform;

    // ReSharper disable InconsistentNaming - Ignored for here, because the AnimationClip needs a public field
    public float X;
    public float Y;
    // ReSharper restore InconsistentNaming
    
    private float _xO;
    private float _yO;
    
    public void Awake()
    {
        _rectTransform = this.GetRequiredComponent<RectTransform>();
    }

    public void Update()
    {
        if (_xO.Equals(X) && _yO.Equals(Y)) return;
        _position.x = _xO = X;
        _position.y = _yO = Y;
        _rectTransform.position = _position;
    }
}