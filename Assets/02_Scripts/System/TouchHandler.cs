using System;
using UnityEngine;

public class TouchHandler : Singleton<TouchHandler>
{
    private GameObject _touchStartGameObject;
    private GameObject _touchEndGameObject;
    private GameObject _touchedGameObject;
    
    private GameObject TouchedGameObject { get; set; }

    public event EventHandler<TouchEvent> Touch;
    
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
        if (!TouchedGameObject) return;
        
        var touched = TouchedGameObject;
        var eventArgs = new TouchEvent(touched);
        Touch?.Invoke(touched, eventArgs);
        var touchables = touched.GetComponents<Touchable>();
        var interfaces = touched.GetComponents<ITouchable>();
        TouchedGameObject = null;

        if (eventArgs.Cancelled) return;
        
        foreach (var touchable in touchables) touchable.InvokeTouch(this, EventArgs.Empty);
        foreach (var @interface in interfaces) @interface.OnTouch();
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
        TouchFeedback.Instance.TryPlayFeedbackAnimation(position);
        if (position == default) return;
        if (!Raycaster.Instance) return; 
        Raycaster.Instance.Raycast(position, out _touchStartGameObject);
        TouchFeedback.Instance.TryPlayShrinkAnimation(_touchStartGameObject);
    }

    private void HandleTouchInputUp()
    {
        if (_touchStartGameObject is null) return;
        if (TouchInputSystem.Instance is null) return;
        if (!TouchInputSystem.Instance.IsFingerUp) return;
        var position = TouchInputSystem.GetTouchPosition();
        if (position == default) return;
        if (!Raycaster.Instance) return;
        Raycaster.Instance.Raycast(position, out _touchEndGameObject);
        TouchFeedback.TryPlayExpandAnimation(_touchEndGameObject);
    }
}