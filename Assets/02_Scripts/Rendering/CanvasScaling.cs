using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class CanvasScaling : MonoBehaviour
{
    private CanvasScaler _canvasScaler;

    [SerializeField] private int _perfectHeight = 1440;
    
    public void Awake()
    {
        _canvasScaler = this.GetRequiredComponent<CanvasScaler>();
    }

    public void Update()
    {
        if (UI.Instance is null) return;
        if (_canvasScaler is null) return;
        
        var deviceHeight = UI.Instance.Canvas.pixelRect.height;
        var scaleFactor = 1.0F / _perfectHeight * deviceHeight;
        _canvasScaler.scaleFactor = scaleFactor;
    }
}