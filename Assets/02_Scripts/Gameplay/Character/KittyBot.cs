using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class KittyBot : Singleton<KittyBot>
{
    private Transform _target;
    private SpriteRenderer _renderer;
    private bool _running;
    private int _animationsComplete;
    private int _previousDecimal;

    private readonly List<KittyBotQueueEntry> _queue = new();

    [Header("Sprites")] [SerializeField] private Sprite _side;
    [SerializeField] private Sprite _back;
    [SerializeField] private Sprite _front;

    [Header("Move Animation")] [Tooltip("A conversion multiplier from UU (unity units) to t (time in ms). UU * x / 1000 = t")] [SerializeField]
    private float _distanceToDurationMod;

    [SerializeField] private AnimationInterpolation _interpolationX;
    [SerializeField] private AnimationInterpolation _interpolationZ;

    [Header("Positions")] [SerializeField] private Transform _waitStation;

    public void OnEnable()
    {
        _renderer = this.GetRequiredComponent<SpriteRenderer>();
        _renderer.sprite = _front;
    }

    public void MoveTo(Transform target, Action callback)
    {
        if (_queue.Any(x => x.Target.gameObject == target.gameObject)) return;
        _queue.Add(new(target, callback, null));
        AddQueueFeedback(target);
        _queue.Add(new(_waitStation, null, null));
    }


    public void Update()
    {
        // Cancel when kitty bot is on it's way back to station and a new order comes in.
        if (_running && _queue.Count > 1 && _queue[0].Callback is null && _queue[0].Animations is not null)
            foreach (var anim in _queue[0].Animations) anim.Stop();
        
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
        UpdateSprite();

        var animationX = AnimationBuilder
            .CreateNew(start.x, end.x, duration)
            .SetInterpolation(_interpolationX)
            .OnUpdate(OnAnimationTickX)
            .OnComplete(OnAnimationComplete)
            .OnlyPlayOnce()
            .Build()
            .Start();

        var animationZ = AnimationBuilder
            .CreateNew(start.z, end.z, duration)
            .SetInterpolation(_interpolationZ)
            .OnUpdate(OnAnimationTickZ)
            .OnComplete(OnAnimationComplete)
            .OnlyPlayOnce()
            .Build()
            .Start();

        _queue[0].Animations = new[] { animationX, animationZ };
    }

    private (Sprite sprite, bool flipped) GetSpriteDataByAngle(float angle)
        => angle switch
        {
            > 0 and <= 45 or > 315 and <= 360 => (_back, false),
            > 45 and <= 135 => (_side, true),
            > 135 and <= 225 => (_front, false),
            > 225 and <= 315 => (_side, false),
            _ => throw new ArgumentOutOfRangeException(nameof(angle), angle, null)
        };

    private void OnAnimationTickX((float c, float t) value) => transform.SetGlobalPositionX(value.c);

    private void OnAnimationTickZ((float c, float t) value)
    {
        transform.SetGlobalPositionZ(value.c);

        // Only calculate when 0.1..., 0.2..., 0.3..., ...
        // 0.4521 -> 4.521 -> "4.521" -> '4' -> "4" -> 4
        var currentDecimal = int.Parse((value.t * 10).ToString(CultureInfo.InvariantCulture)[0].ToString());
        if (currentDecimal == _previousDecimal || currentDecimal == 1) return;
        _previousDecimal = currentDecimal;
        UpdateSprite();
    }

    private void OnAnimationComplete()
    {
        if (++_animationsComplete < 2) return;
        RemoveQueueFeedback();
        CompleteQueueEntry();
        ResetSprite();

        _running = false;
    }

    private void CompleteQueueEntry()
    {
        try
        {
            if (!_queue[0].Target.gameObject) return;
            _queue[0].Callback?.Invoke();
        }
        catch (Exception ex)
        {
            Debug.LogError(new Exception("An error occurred when invoking the callback of the KittyBot move animation.", ex));
        }
        finally
        {
            _queue.Remove(_queue[0]);
        }
    }

    private void ResetSprite()
    {
        if (_queue.Count != 0) return;
        _renderer.sprite = _front;
        _renderer.flipX = false;
    }

    private void UpdateSprite()
    {
        var rotation = transform.GetZAngleTo(_target.position);
        var (sprite, flipped) = GetSpriteDataByAngle(rotation);
        _renderer.sprite = sprite;
        _renderer.flipX = flipped;
    }

    private static void AddQueueFeedback(Transform target)
    {
        var queueFeedback = target.GetComponentInChildren<QueueFeedback>();
        if (!queueFeedback) return;
        queueFeedback.Show();
    }

    private void RemoveQueueFeedback()
    {
        var queueFeedback = _queue[0].Target.GetComponentInChildren<QueueFeedback>();
        if (!queueFeedback) return;
        queueFeedback.Hide();
    }
}

internal class KittyBotQueueEntry
{
    public KittyBotQueueEntry(Transform target, Action callback, AnimationBuilder[] animations)
    {
        Target = target; 
        Callback = callback;
        Animations = animations;
    }
    
    public Transform Target { get; set; }
    public Action Callback { get; set; }
    public AnimationBuilder[] Animations { get; set; }
}