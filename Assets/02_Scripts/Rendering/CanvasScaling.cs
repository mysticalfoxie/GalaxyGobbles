using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class CanvasScaling : MonoBehaviour
{
    private CanvasScaler _canvasScaler;
    private int _height;

    [SerializeField] private int _perfectHeight = 1440;

    public static float ScaleFactor { get; private set; }
    
    public void Awake()
    {
        _canvasScaler = this.GetRequiredComponent<CanvasScaler>();
    }

    public void Update()
    {
        if (!UI.Instance) return;
        if (_canvasScaler is null) return;
        if (Screen.height == _height) return;
        _height = Screen.height;
        
        var deviceHeight = UI.Instance.Canvas.pixelRect.height;
        ScaleFactor = 1.0F / _perfectHeight * deviceHeight;
        _canvasScaler.scaleFactor = ScaleFactor;
        
        Debug.Log($"[Canvas Scaling] Detected screen size change. Adjusting global canvas scale to {_canvasScaler.scaleFactor}.");
    }
}