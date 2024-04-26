using System;
using UnityEngine;

public class TouchHandler : SingletonMonoBehaviour<TouchHandler>
{
    private GameObject _touchStartGameObject;
    private GameObject _touchEndGameObject;
    private GameObject _touchedGameObject;
    private Camera _camera;
    
    private GameObject TouchedGameObject { get; set; }
    
    [Header("Touch Detection System")]
    [SerializeField]
    [Range(1, 100)]
    private float _raycastMaxRange = 20.0F;

    public event EventHandler Touch;

    public override void Awake()
    {
        base.Awake();
        _camera = Camera.main;
    }
    
    public void Update()
    {
        UpdateStatesAndCallEvents();
        HandleTouchInputDown();
        HandleTouchInputUp();
        ClearTouchStateCache();
        CheckTouchState();
        ClearUpDownStateCache();
    }

    private void UpdateStatesAndCallEvents()
    {
        if (TouchedGameObject is null) return;

        var touched = TouchedGameObject;
        Touch?.Invoke(touched, EventArgs.Empty);
        var touchables = touched.GetComponents<TouchableMonoBehaviour>();
        TouchedGameObject = null;
        
        if (touchables.Length == 0) return;
        
        foreach (var touchable in touchables)
            touchable.InvokeTouch(this, EventArgs.Empty);
    }

    private void CheckTouchState()
    {
        if (_touchStartGameObject is null) return;
        if (_touchEndGameObject is null) return;
        if (_touchEndGameObject != _touchStartGameObject) return;
        _touchedGameObject = _touchStartGameObject; 
    }

    private void ClearTouchStateCache()
    {
        if (_touchedGameObject is null) return;
        TouchedGameObject = _touchedGameObject;
        _touchedGameObject = null; 
    }

    private void ClearUpDownStateCache()
    {
        if (_touchStartGameObject is null) return;
        if (_touchEndGameObject is null) return;
        
        _touchStartGameObject = null;
        _touchEndGameObject = null;
    }
    
    private void HandleTouchInputDown()
    {
        if (TouchInputSystem.Instance is null) return;
        if (!TouchInputSystem.Instance.IsFingerDown) return;
        var position = TouchInputSystem.GetTouchPosition();
        if (position == default) return;
        _touchStartGameObject = RaycastGameObject(position);
    }

    private void HandleTouchInputUp()
    {
        if (_touchStartGameObject is null) return;
        if (!TouchInputSystem.Instance.IsFingerUp) return;
        var position = TouchInputSystem.GetTouchPosition();
        if (position == default) return;
        
        _touchEndGameObject = RaycastGameObject(position);
    }
    
    
    public GameObject RaycastGameObject(Vector2 touchPosition)
    {
        if (_camera is null) return null;
        
        var ray = _camera.ScreenPointToRay(touchPosition);
        Physics.Raycast(ray, out var raycast, _raycastMaxRange);
        
        if (raycast.transform is null) return null;
        if (raycast.collider is null) return null;

        return raycast.collider.gameObject;
    }
}