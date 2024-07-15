using System;
using UnityEngine;

public class MainCharacter : Singleton<MainCharacter>
{
    private Transform _target;
    private Vector3 _maxPosition;
    private Vector3 _minPosition;
    private SpriteRenderer _renderer;
    private Action _callback;

    [Header("Sprites")] 
    [SerializeField] private Sprite _side;
    [SerializeField] private Sprite _back;
    [SerializeField] private Sprite _front;
    
    [Header("Move Animation")]
    [Tooltip("A conversion multiplier from UU (unity units) to t (time in ms). UU * x / 1000 = t")]
    [SerializeField] private float _distanceToDurationMod;
    [SerializeField] private AnimationInterpolation _interpolation;
    
    [Header("Bounds")]
    [SerializeField] private float _maxX;
    [SerializeField] private float _minX;
    [SerializeField] private float _gizmoY;

    public override void Awake()
    {
        InheritedDDoL = true;
        base.Awake();
    }

    public void OnEnable()
    {
        _maxPosition = transform.TransformPoint(new Vector3(_maxX, 0, 0));
        _minPosition = transform.TransformPoint(new Vector3(_minX, 0, 0));
        _renderer = this.GetRequiredComponent<SpriteRenderer>();
        _renderer.sprite = _front;
    }

    public void MoveTo(Transform target, Action callback)
    {
        _target = target;
        _callback = callback;
        var start = transform.position.x;
        var end = ClampTargetPosition(target).x;
        var distance = Mathf.Abs(end - start);
        var duration = distance * _distanceToDurationMod / 1000;

        AnimationBuilder
            .CreateNew(start, end, duration)
            .SetInterpolation(_interpolation)
            .OnUpdate(OnAnimationTick)
            .OnComplete(OnAnimationComplete)
            .OnlyPlayOnce()
            .Build()
            .Start();
    }

    private void OnAnimationTick(float value)
    {
        transform.SetGlobalPositionX(value);
        _renderer.sprite = _side;
        _renderer.flipX = _target.position.x > transform.position.x;
        
    }

    private void OnAnimationComplete()
    {
        _renderer.sprite = _target.position.z > transform.position.z ? _back : _front;
        _renderer.flipX = false;
        _callback?.Invoke();
    }


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        var p1 = new Vector3(_minX, _gizmoY, transform.localPosition.z);
        var p2 = new Vector3(_maxX, _gizmoY, transform.localPosition.z);
        Gizmos.DrawLine(transform.TransformPoint(p1), transform.TransformPoint(p2));
    }
    

    private Vector3 ClampTargetPosition(Transform target)
    {
        var cx = Mathf.Min(Mathf.Max(target.position.x, _minPosition.x), _maxPosition.x);
        return target.position.SetX(cx);
    }
}
