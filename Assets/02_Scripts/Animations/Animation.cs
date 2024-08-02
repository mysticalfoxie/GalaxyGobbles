using System;
using JetBrains.Annotations;
using UnityEngine;

public class Animation : IDisposable
{
    [UsedImplicitly] private Guid _id;

    private readonly float _a; // State A
    private readonly float _b; // State B
    private readonly float _d; // Duration
    private readonly AnimationInterpolation _i; // Interpolation
    private Action _cb; // Callback
    private float _t; // Current Time
    private float _c; // Current Value

    public Animation(float a, float b, float d, AnimationInterpolation i)
    {
        _a = a;
        _b = b;
        _d = d;
        _i = i;
        
        AnimationHandler.Instance.Tick += OnTick;
        _id = Guid.NewGuid();
    }

    public bool IsPlaying { get; private set; }

    public event EventHandler<(float c, float t)> Tick;
    public event EventHandler Complete;
    public event EventHandler Disposed;

    private void OnTick(object sender, EventArgs e)
    {
        if (!IsPlaying) return;
        if (_t >= _d)
        {
            IsPlaying = false;
            Complete?.Invoke(this, EventArgs.Empty);

            return;
        }
        
        NextAnimationFrame();
    }

    private void NextAnimationFrame()
    {
        _t += Time.deltaTime;
        var t = Mathf.Max(Mathf.Min(1 / _d * _t, 1), 0);
        var x = AnimationFunctions.Interpolate(_i, _a, _b, t);
        _c = _a > _b ? Mathf.Max(x, _b) : Mathf.Min(x, _b);
        Tick?.Invoke(this, (_c, _t));
    }

    public void Start()
    {
        IsPlaying = true;
        _c = _a;
        _t = 0;
    }

    public void Dispose()
    {
        AnimationHandler.Instance.Tick -= OnTick;
        GC.SuppressFinalize(this);
        IsPlaying = false;
        Disposed?.Invoke(this, EventArgs.Empty);
    }
}
