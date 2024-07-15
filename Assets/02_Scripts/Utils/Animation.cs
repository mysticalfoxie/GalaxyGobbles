using System;
using UnityEngine;

public class Animation : IDisposable
{
    private readonly float _a;
    private readonly float _b;
    private readonly float _d;
    private readonly AnimationInterpolation _i;
    private readonly Func<(float a, float b, float c, float t), bool> _ccc;
    private Action _cb;
    private float _t;
    private float _c;
    private bool _running;
    
    public Animation(float a, float b, float d, AnimationInterpolation i, Func<(float a, float b, float c, float t), bool> ccc)
    {
        _a = a;
        _b = b;
        _d = d;
        _i = i;
        _ccc = ccc;
        
        AnimationHandler.Instance.Tick += OnTick;
    }

    public event EventHandler<float> Tick;
    public event EventHandler Complete;
    public event EventHandler Disposed;

    private void OnTick(object sender, EventArgs e)
    {
        if (!_running) return;
        if (IsComplete())
        {
            Complete?.Invoke(this, EventArgs.Empty);
            _running = false;
            return;
        }
        
        NextAnimationFrame();
    }

    private bool IsComplete()
    {
        if (_ccc is not null)
            return _ccc((_a, _b, _c, _t));
        return _a > _b ? _c <= _b : _c >= _b;
    }

    private void NextAnimationFrame()
    {
        _t += Time.deltaTime;
        var t = Mathf.Max(Mathf.Min(1 / _d * _t, 1), 0);
        var x = AnimationFunctions.Interpolate(_i, _a, _b, t);
        _c = _a > _b ? Mathf.Max(x, _b) : Mathf.Min(x, _b);
        Tick?.Invoke(this, _c);
    }

    public void Start()
    {
        _running = true;
        _c = _a;
    }

    public void Dispose()
    {
        AnimationHandler.Instance.Tick -= OnTick;
        GC.SuppressFinalize(this);
        _running = false;
        Disposed?.Invoke(this, EventArgs.Empty);
    }
}