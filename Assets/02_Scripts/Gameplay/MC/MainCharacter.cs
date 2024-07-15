using System;
using UnityEngine;

public class MainCharacter : Singleton<MainCharacter>
{
    private float? _targetX;
    private float? _startX;
    private float? _distance;
    private float? _timeElapsed;
    private float? _duration;
    private Action _callback;

    public override void Awake()
    {
        InheritedDDoL = true;
        base.Awake();
    }
    
    [Header("Move Animation")]
    [Tooltip("A conversion multiplier from UU (unity units) to t (time in ms). UU * x / 1000 = t")]
    [SerializeField] private float _distanceToDurationMod;
    [SerializeField] private AnimationInterpolation _interpolation;
    
    [Header("Bounds")]
    [SerializeField] private float _maxX;
    [SerializeField] private float _minX;
    [SerializeField] private float _gizmoY;
    private Vector3 _maxPosition;
    private Vector3 _minPosition;

    public void OnEnable()
    {
        _maxPosition = transform.TransformPoint(new Vector3(_maxX, 0, 0));
        _minPosition = transform.TransformPoint(new Vector3(_minX, 0, 0));
    }

    public void MoveTo(Transform target, Action callback)
    {
        var start = transform.position.x;
        var end = ClampTargetPosition(target).x;
        var distance = Mathf.Abs(end - start);
        var duration = distance * _distanceToDurationMod / 1000;

        AnimationBuilder
            .CreateNew(start, end, duration)
            .SetInterpolation(_interpolation)
            .OnUpdate(x => transform.SetGlobalPositionX(x))
            .OnComplete(callback)
            .OnlyPlayOnce()
            .Build()
            .Start();
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
