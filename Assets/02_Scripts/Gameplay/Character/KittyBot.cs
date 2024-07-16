using System;
using System.Collections.Generic;
using UnityEngine;

public class KittyBot : Singleton<KittyBot>
{
    private Transform _target;
    private SpriteRenderer _renderer;
    private bool _running;
    private int _animationsComplete;

    private readonly List<(Transform Target, Action Callback)> _queue = new();
    
    [Header("Sprites")] 
    [SerializeField] private Sprite _side;
    [SerializeField] private Sprite _back;
    [SerializeField] private Sprite _front;
    
    [Header("Move Animation")]
    [Tooltip("A conversion multiplier from UU (unity units) to t (time in ms). UU * x / 1000 = t")]
    [SerializeField] private float _distanceToDurationMod;
    [SerializeField] private AnimationInterpolation _interpolationX;
    [SerializeField] private AnimationInterpolation _interpolationZ;

    [Header("Positions")]
    [SerializeField] private Transform _waitStation;
    
    public void OnEnable()
    {
        _renderer = this.GetRequiredComponent<SpriteRenderer>();
        _renderer.sprite = _front;
    }

    public void MoveTo(Transform target, Action callback)
    {
        _queue.Add((target, callback));
        AddQueueFeedback(target);
        _queue.Add((_waitStation, null));
    }
    
    private static void AddQueueFeedback(Transform target)
    {
        var queueFeedback = target.GetComponentInChildren<QueueFeedback>();
        if (!queueFeedback) return;
        queueFeedback.Show();
    }

    public void Update()
    {
        if (_queue.Count > 0 && !_running)
            StartAnimation();
    }

    private void StartAnimation()
    {
        _running = true;
        _animationsComplete = 0;
        _target = _queue[0].Target;
        
        var start = transform.position;
        var end = _target.position;
        var distance = Vector3.Distance(start, end); // Thanks, Pythagoras buddy ;) 
        var duration = distance * _distanceToDurationMod / 1000;
        // Calculate look angle
        
        AnimationBuilder
            .CreateNew(start.x, end.x, duration)
            .SetInterpolation(_interpolationX)
            .OnUpdate(OnAnimationTickX)
            .OnComplete(OnAnimationComplete)
            .OnlyPlayOnce()
            .Build()
            .Start();
        
        AnimationBuilder
            .CreateNew(start.z, end.z, duration)
            .SetInterpolation(_interpolationZ)
            .OnUpdate(OnAnimationTickZ)
            .OnComplete(OnAnimationComplete)
            .OnlyPlayOnce()
            .Build()
            .Start();
    }

    private void OnAnimationTickX(float value)
    {
        transform.SetGlobalPositionX(value);
        // _renderer.sprite = _side;
        // _renderer.flipX = _target.position.x > transform.position.x;
    }

    private void OnAnimationTickZ(float value)
    {
        transform.SetGlobalPositionZ(value);
    }
    
    private void OnAnimationComplete()
    {
        if (++_animationsComplete < 2) return;
        _renderer.sprite = _target.position.z > transform.position.z ? _back : _front;
        _renderer.flipX = false;
        RemoveQueueFeedback();
        _queue[0].Callback?.Invoke();
        _queue.Remove(_queue[0]);
        _running = false;
    }

    private void RemoveQueueFeedback()
    {
        var queueFeedback = _queue[0].Target.GetComponentInChildren<QueueFeedback>();
        if (!queueFeedback) return;
        queueFeedback.Hide();
    }
}