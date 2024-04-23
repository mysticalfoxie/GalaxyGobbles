using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public sealed class TouchEventSystem : SingletonMonoBehaviour<TouchEventSystem>, ITouchEventSystem
{
    public const string INPUT_PRESS = "Press";          //  Button down and Button up -> Press performed
    public const string INPUT_TAP = "Tap";              //  Only a click within .4s is a tap 
    public const string INPUT_POSITION = "Position";    //  Triggers with each position change
    
    private PlayerInput _input;
    
    [Header("Touch Detection System")]
    [SerializeField]
    [Range(1, 100)]
    private float _raycastMaxRange = 20.0F;
    
    private bool _tap;
    private bool _fingerDown;
    private bool _fingerPressing;
    private bool _fingerUp;
    private bool _moving;
    
    private InputAction.CallbackContext? _tapContext;
    private InputAction.CallbackContext? _fingerDownContext;
    private InputAction.CallbackContext? _fingerUpContext;
    private InputAction.CallbackContext? _movingContext;
    private GameObject _tappedObjectCache;
    
    public bool IsTapping { get; private set; }
    public bool IsFingerDown { get; private set; }
    public bool IsFingerPressing { get; private set; }
    public bool IsFingerUp { get; private set; }
    public bool IsFingerMoving { get; private set; }
    
    public InputAction.CallbackContext? TapContext { get; private set; }
    public InputAction.CallbackContext? FingerDownContext { get; private set; }
    public InputAction.CallbackContext? FingerUpContext { get; private set; }
    public InputAction.CallbackContext? MovingContext { get; private set; }
    
    protected TouchEventSystem() : base(true) { }
    
    public override void Awake()
    {
        base.Awake();
        
        EnhancedTouchSupport.Enable();
        
        _input = this.GetRequiredComponent<PlayerInput>();
        
        _input.actions[INPUT_PRESS].started += OnFingerDown;
        _input.actions[INPUT_PRESS].canceled += OnFingerUp;
        _input.actions[INPUT_POSITION].performed += OnPositionChanged;
        _input.actions[INPUT_TAP].performed += OnTapped;
    }

    private void OnTapped(InputAction.CallbackContext context)
    {
        _tap = true;
        _tapContext = context;
        
        Debug.Log(nameof(OnTapped));
    }

    private void OnPositionChanged(InputAction.CallbackContext context)
    {
        _moving = true;
        _movingContext = context;
        
        Debug.Log(nameof(OnPositionChanged));
    }

    private void OnFingerUp(InputAction.CallbackContext context)
    {
        _fingerUp = true;
        _fingerUpContext = context;
        
        Debug.Log(nameof(OnFingerUp));
    }

    private void OnFingerDown(InputAction.CallbackContext context)
    {
        _fingerDown = true;
        _fingerDownContext = context;
        
        Debug.Log(nameof(OnFingerDown));
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

        _tappedObjectCache = null;
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

    public GameObject GetTappedGameObject()
    {
        if (!IsTapping) return null;
        if (TapContext is null) return null;

        var mainCamera = Camera.main;
        
        if (mainCamera is null) return null;
        if (_tappedObjectCache is not null) return _tappedObjectCache;

        var position = GetTouchPosition();
        var ray = mainCamera.ScreenPointToRay(position);
        Physics.Raycast(ray, out var raycast, _raycastMaxRange);
        
        if (raycast.transform is null) return null;
        if (raycast.collider is null) return null;

        return _tappedObjectCache = raycast.collider.gameObject;
    }

    private static Vector2 GetTouchPosition()
    {
        var touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        if (touchPosition != default) return touchPosition;
        touchPosition = Mouse.current.position.ReadValue();
        if (touchPosition != default) return touchPosition;
        return Pen.current.position.ReadValue();
    }

}
