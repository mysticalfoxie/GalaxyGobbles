using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchFeedback : Singleton<TouchFeedback>
{
    public TouchFeedback() : base(true) { }

    [SerializeField] private GameObject _touchAnimation;
    private GameObject _pushedGameObject;
    private bool _pushed;

    public void Start()
    {
        TouchInputSystem.Instance.Move += OnMove;
    }

    public void TryPlayFeedbackAnimation(Vector2 position)
    {
        try
        {
            PlayFeedbackAnimation(position);
        }
        catch (Exception ex)
        {
            Debug.LogError(new Exception("An error occurred when playing the touch feedback animation.", ex));
        }
    }

    private void PlayFeedbackAnimation(Vector2 position)
    {
        var touchAnimation = Instantiate(_touchAnimation);
        var rectTransform = touchAnimation.GetRequiredComponent<RectTransform>();
        touchAnimation.transform!.SetParent(UI.Instance.transform, true);
        rectTransform.position = position;
    }

    public void TryPlayShrinkAnimation(GameObject pushedGameObject)
    {
        if (!pushedGameObject) return;
        var animator = pushedGameObject.GetComponent<TouchAnimator>()
                       ?? pushedGameObject.GetComponentInChildren<TouchAnimator>();
        if (!animator) return;
        animator.Shrink();
        _pushedGameObject = pushedGameObject;
    }

    public static void TryPlayExpandAnimation(GameObject releasedGameObject)
    {
        if (!releasedGameObject) return;
        var animator = releasedGameObject.GetComponent<TouchAnimator>() 
                       ?? releasedGameObject.GetComponentInChildren<TouchAnimator>();
        if (!animator) return;
        animator.Expand();
    }

    private void OnMove(object sender, InputAction.CallbackContext context)
    {
        if (_pushedGameObject is null) return;
        var position = TouchInputSystem.GetTouchPosition();
        if (position == default) return;
        if (!Raycaster.Instance) return;
        Raycaster.Instance.Raycast(position, out var touched);
        
        if (touched != _pushedGameObject && _pushed)
        {
            TryPlayExpandAnimation(_pushedGameObject);
            _pushed = false;
        }
        else if (touched == _pushedGameObject && !_pushed)
        {
            TryPlayShrinkAnimation(_pushedGameObject);
            _pushed = true;
        }
    }

    public void InvokeReleaseEvent(GameObject releasedObject)
    {
        if (!releasedObject) return;
        var touchables = releasedObject.GetComponents<Touchable>();
        var interfaces = releasedObject.GetComponents<ITouchable>();
        foreach (var touchable in touchables) touchable.InvokeRelease(this, EventArgs.Empty);
        foreach (var @interface in interfaces) @interface.OnRelease();
    }

    public void InvokePushEvent(GameObject pushedObject)
    {
        if (!pushedObject) return;
        var touchables = pushedObject.GetComponents<Touchable>();
        var interfaces = pushedObject.GetComponents<ITouchable>();
        foreach (var touchable in touchables) touchable.InvokePush(this, EventArgs.Empty);
        foreach (var @interface in interfaces) @interface.OnPush();
    }
}

