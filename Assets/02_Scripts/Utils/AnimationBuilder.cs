using System;


public class AnimationBuilder
{
    private float _a;
    private float _b;
    private float _d = 1.0F;
    private AnimationInterpolation _i = AnimationInterpolation.EaseInOutCubic;
    private Func<(float a, float b, float c, float t), bool> _ccc;
    private bool _playOnce;
    private bool _looped;
    private Animation _animation;
    private Action _completeCallback;
    private Action _disposedCallback;
    private Action<(float c, float t)> _updateCallback;

    private AnimationBuilder() { }
    
    public static AnimationBuilder CreateNew()
    {
        return new AnimationBuilder();
    }
    
    public static AnimationBuilder CreateNew(float a, float b, float d)
    {
        return new AnimationBuilder()
            .From(a).To(b).SetDuration(d);
    }

    public AnimationBuilder From(float start)
    {
        _a = start;
        return this;
    }

    public AnimationBuilder To(float target)
    {
        _b = target;
        return this;
    }
    
    public AnimationBuilder SetDuration(float duration)
    {
        _d = duration;
        return this;
    }

    public AnimationBuilder SetLooped()
    {
        _looped = true;
        return this;
    }

    public AnimationBuilder SetInterpolation(AnimationInterpolation interpolation)
    {
        _i = interpolation;
        return this;
    }

    public AnimationBuilder SetCustomCompleteCheck(Func<(float a, float b, float c, float t), bool> completeCheck)
    {
        _ccc = completeCheck;
        return this;
    }

    public AnimationBuilder OnlyPlayOnce()
    {
        _playOnce = true;
        return this;
    }

    public AnimationBuilder OnDisposed(Action callback)
    {
        _disposedCallback = callback;
        return this;
    }

    public AnimationBuilder OnUpdate(Action<(float c, float t)> callback)
    {
        _updateCallback = callback;
        return this;
    }

    public AnimationBuilder OnComplete(Action callback)
    {
        _completeCallback = callback;
        return this;
    }

    public AnimationBuilder Build()
    {
        if (_animation is not null) throw new InvalidOperationException("The animation has already been built.");
        _animation = new Animation(_a, _b, _d, _i, _ccc);
        _animation.Complete += OnAnimationComplete;
        _animation.Disposed += OnAnimationDisposed;
        _animation.Tick += OnAnimationTick;
        return this;
    }

    public AnimationBuilder Start()
    {
        if (_animation is null) throw new InvalidOperationException("The animation has not been built yet.");
        _animation.Start();
        return this;
    }

    public void Stop()
    {
        if (_animation is null) return;
        _animation.Dispose();
        _completeCallback?.Invoke();
    }

    private void OnAnimationTick(object sender, (float c, float t) value)
    {
        _updateCallback?.Invoke(value);
    }

    private void OnAnimationComplete(object sender, EventArgs e)
    {
        if (_looped)
        {
            _animation.Start();
            return;
        }
        
        _completeCallback?.Invoke();
        
        if (!_playOnce) return;
        _animation.Complete -= OnAnimationComplete;
        _animation.Dispose();
    }

    private void OnAnimationDisposed(object sender, EventArgs e)
    {
        _disposedCallback?.Invoke();
        _animation = null;
    }
}