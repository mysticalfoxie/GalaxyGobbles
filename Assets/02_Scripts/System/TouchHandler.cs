using System;
using UnityEngine;

public class TouchHandler : SingletonMonoBehaviour<TouchHandler>
{
    private GameObject _touchStartGameObject;
    private GameObject _touchEndGameObject;
    private GameObject _touchedGameObject;
    private Camera _camera;
    
    private GameObject TouchedGameObject { get; set; }

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
        
        try
        {
            // Provoking a UnityEngine.UnassignedReferenceException
            TouchedGameObject.SetActive(true);
        }
        catch (UnassignedReferenceException)
        {
            Debug.LogWarning("UnassignedReferenceException. Cannot update states and call events for an unassigned gameobject.");
            TouchedGameObject = null;
            return;
        }
        
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
        DimensionHelper.Instance.Convert2Dto3D(position, out _touchStartGameObject);
        EnsureObjectAssigned(_touchStartGameObject, () => _touchStartGameObject = null);
    }

    private static void EnsureObjectAssigned(GameObject @object, Func<GameObject> onError)
    {
        try
        {
            // Provoking a UnityEngine.UnassignedReferenceException
            @object.SetActive(true);
        }
        catch (UnassignedReferenceException)
        {
            Debug.LogWarning("UnassignedReferenceException. The player touched an unassigned game object.");
            onError();
        }
    }

    private void HandleTouchInputUp()
    {
        if (_touchStartGameObject is null) return;
        if (!TouchInputSystem.Instance.IsFingerUp) return;
        var position = TouchInputSystem.GetTouchPosition();
        if (position == default) return;

        DimensionHelper.Instance.Convert2Dto3D(position, out _touchEndGameObject);
        EnsureObjectAssigned(_touchEndGameObject, () => _touchEndGameObject = null);
    }
}