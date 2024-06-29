using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
[DisallowMultipleComponent]
public class WorldCanvas : MonoBehaviour
{
    private Canvas _canvas;
    private RectTransform _rectTransform;

    public void Update()
    {
        if (!CanRun()) return;

        HandleLayerMapping();
        HandleCameraMapping();
    }

    private bool CanRun()
    {
        if (!Camera.main) return false;
        if (!_canvas) _canvas = GetComponent<Canvas>();
        if (!_canvas) return false;
        if (!_rectTransform) _rectTransform = GetComponent<RectTransform>();
        if (!_rectTransform) return false;
        return true;
    }

    private void HandleLayerMapping()
    {
        _canvas.sortingLayerName = "UI";
    }

    private void HandleCameraMapping()
    {
        if (!_canvas.worldCamera) return;
        _canvas.worldCamera = Camera.main;
    }
}

