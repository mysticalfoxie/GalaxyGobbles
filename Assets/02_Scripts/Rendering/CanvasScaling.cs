using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class CanvasScaling : MonoBehaviour
{
    private CanvasScaler _canvasScaler;
    private int _height;

    [SerializeField] private int _perfectHeight = 1440;
    
    public void Awake()
    {
        _canvasScaler = this.GetRequiredComponent<CanvasScaler>();
    }

    public void Update()
    {
        if (!UI.Instance.IsAssigned()) return;
        if (_canvasScaler is null) return;
        if (Screen.height == _height) return;
        _height = Screen.height;
        
        var deviceHeight = UI.Instance.Canvas.pixelRect.height;
        var scaleFactor = 1.0F / _perfectHeight * deviceHeight;
        _canvasScaler.scaleFactor = scaleFactor;
        
        Debug.Log($"[Canvas Scaling] Detected screen size change. Adjusting global canvas scale to {_canvasScaler.scaleFactor}.");
    }
}