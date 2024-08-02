using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public sealed class TouchInputSystem : Singleton<TouchInputSystem>
{
    public const string INPUT_PRESS = "Press";          //  Button down and Button up -> Press performed
    public const string INPUT_TAP = "Tap";              //  Only a click within .4s is a tap 
    public const string INPUT_POSITION = "Position";    //  Triggers with each position change
    
    private PlayerInput _input;
    
    private bool _tap;
    private bool _fingerDown;
    private bool _fingerPressing;
    private bool _fingerUp;
    private bool _moving;
    
    private InputAction.CallbackContext? _tapContext;
    private InputAction.CallbackContext? _fingerDownContext;
    private InputAction.CallbackContext? _fingerUpContext;
    private InputAction.CallbackContext? _movingContext;
    
    public bool IsTapping { get; private set; }
    public bool IsFingerDown { get; private set; }
    public bool IsFingerPressing { get; private set; }
    public bool IsFingerUp { get; private set; }
    public bool IsFingerMoving { get; private set; }
    
    public InputAction.CallbackContext? TapContext { get; private set; }
    public InputAction.CallbackContext? FingerDownContext { get; private set; }
    public InputAction.CallbackContext? FingerUpContext { get; private set; }
    public InputAction.CallbackContext? MovingContext { get; private set; }

    public event EventHandler<InputAction.CallbackContext> Move;
    public event EventHandler<InputAction.CallbackContext> Tap;
    public event EventHandler<InputAction.CallbackContext> Push;
    public event EventHandler<InputAction.CallbackContext> Release;
    
    public override void Awake()
    {
        base.Awake();
        
        EnhancedTouchSupport.Enable();
        
        _input = this.GetRequiredComponent<PlayerInput>();
        
        _input.actions[INPUT_PRESS].started += OnPush;
        _input.actions[INPUT_PRESS].canceled += OnRelease;
        _input.actions[INPUT_POSITION].performed += OnMove;
        _input.actions[INPUT_TAP].performed += OnTapped;
    }

    private void OnTapped(InputAction.CallbackContext context)
    {
        _tap = true;
        _tapContext = context;
        Tap?.Invoke(this, context);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moving = true;
        _movingContext = context;
        Move?.Invoke(this, context);
    }

    private void OnRelease(InputAction.CallbackContext context)
    {
        _fingerUp = true;
        _fingerUpContext = context;
        Release?.Invoke(this, context);
    }

    private void OnPush(InputAction.CallbackContext context)
    {
        _fingerDown = true;
        _fingerDownContext = context;
        Push?.Invoke(this, context);
    }

    public void Update()
    {
        // Applies the states from cache and resets the cache
        UpdateCurrentFrameStates();
        ResetCachedStates();
    }

    private void ResetCachedStates()
    {
        _tap = false;
        _fingerDown = false;
        _fingerUp = false;
        _moving = false;

        _tapContext = null;
        _fingerDownContext = null;
        _fingerUpContext = null;
        _movingContext = null;
    }

    private void UpdateCurrentFrameStates()
    {
        IsTapping = _tap;
        IsFingerDown = _fingerDown;
        IsFingerUp = _fingerUp;
        IsFingerPressing = _fingerPressing;
        IsFingerMoving = _moving;

        TapContext = _tapContext;
        FingerDownContext = _fingerDownContext;
        FingerUpContext = _fingerUpContext;
        MovingContext = _movingContext;

        if (_fingerDown) _fingerPressing = true;
        if (_fingerUp) _fingerPressing = false;
    }

    public static Vector2 GetTouchPosition()
    {
        var touchPosition = Touchscreen.current is not null 
            && Touchscreen.current.primaryTouch is not null
            ? Touchscreen.current.primaryTouch.position.ReadValue() : default;
        if (touchPosition != default) return touchPosition;
        touchPosition = Mouse.current is not null ? Mouse.current.position.ReadValue() : default;
        if (touchPosition != default) return touchPosition;
        return Pen.current is not null ? Pen.current.position.ReadValue() : default;
    }

}
