using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class PositionCalculator2D : MonoBehaviour
{
    private RectTransform _rectTransform;
    
    [Header("Positions")]
    [SerializeField] private bool _lockPositionBefore;
    [SerializeField] private Vector2 _positionBefore;
    [SerializeField] private Vector2 _positionAfter;
    
    [Header("Result")] 
    [SerializeField] private Vector2 _change;
    
#if UNITY_EDITOR
    public void Update()
    {
        if (!gameObject) return;
        if (!isActiveAndEnabled) return;
        if (!_rectTransform) _rectTransform = GetComponent<RectTransform>();
        if (!_lockPositionBefore)
        {
            _positionBefore = _rectTransform.localPosition;
            return;
        }

        _positionAfter = _rectTransform.localPosition;
        var x = _positionAfter.x - _positionBefore.x;
        var y = _positionAfter.y - _positionBefore.y;
        _change = new Vector2(x, y);
    }
#endif
}