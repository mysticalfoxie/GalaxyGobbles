using System;

public class AnimationBuilder
{
    private float _a;
    private float _b;
    private float _d = 1.0F;
    private AnimationInterpolation _i = AnimationInterpolation.EaseInOutCubic;
    private bool _playOnce;
    private bool _looped;
    private bool _unscaledTime;
    private Animation _animation;
    private Action _completeCallback;
    private Action _disposedCallback;
    private Action<(float c, float t)> _updateCallback;

    public bool IsPlaying => _animation?.IsPlaying ?? false;

    private AnimationBuilder()
    {
    }
    
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

    public AnimationBuilder SetLooped(bool value = true)
    {
        _looped = value;
        return this;
    }

    public AnimationBuilder SetInterpolation(AnimationInterpolation interpolation)
    {
        _i = interpolation;
        return this;
    }

    public AnimationBuilder SetPlayOnce(bool playOnce = true)
    {
        _playOnce = playOnce;
        return this;
    }

    public AnimationBuilder SetUnscaledTime(bool unscaledTime = true)
    {
        _unscaledTime = unscaledTime;
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
        _animation = new Animation(_a, _b, _d, _i, _unscaledTime);
        _animation.Complete += OnAnimationComplete;
        _animation.Disposed += OnAnimationDisposed;
        _animation.Tick += OnAnimationTick;
        return this;
    }

    public bool IsDisposed() => _animation is null;

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

