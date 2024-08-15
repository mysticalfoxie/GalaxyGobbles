using UnityEngine;

public abstract class AnimatorBase : MonoBehaviour
{
    protected AnimationBuilder _animation;
    
    [Header("Animation")] [Tooltip("The duration of the chair pulsing animation.")] [SerializeField] [Range(0.1F, 10.0F)]
    protected float _duration = 1.0F;

    [Tooltip("The scale increase and decrease as a factor.")] [SerializeField] [Range(0.1F, 10.0F)]
    protected float _strength = 1.0F;

    [Tooltip("The multipliers for each axis. 1.0F = fully affected, 0.0F = static - not moving.")] [SerializeField]
    protected  Vector3 _weight = new(1.0F, 1.0F, 1.0F);

    [SerializeField] protected bool _unscaledTime;
    [SerializeField] protected bool _playOnAwake;
    [SerializeField] protected bool _playOnce;
    [SerializeField] protected bool _looped;

    public bool Playing => _animation?.IsPlaying ?? false;

    public bool UnscaledTime
    {
        get => _unscaledTime;
        set => _unscaledTime = value;
    }
    
    public float Strength
    {
        get => _strength;
        set => _strength = value;
    }

    public Vector3 Weight
    {
        get => _weight;
        set => _weight = value;
    }

    public float Duration
    {
        get => _duration;
        set => _duration = value;
    }

    public bool Looped
    {
        get => _looped;
        set => _looped = value;
    }

    public virtual void Awake()
    {
        Configure();
        if (!_playOnAwake) return;
        Play();
    }

    public virtual void Play()
    {
        if (!this || !isActiveAndEnabled) return;
        if (_animation is null || _animation.IsDisposed()) Configure();
        if (_animation!.IsPlaying) return;
        _animation!.Start();
    }

    public virtual void Stop()
    {
        if (_animation is null) return;
        _animation.Stop();
        _animation = null;
    }

    public void UpdateConfiguration()
    {
        if (_animation?.IsPlaying ?? false) _animation.Stop();
        _animation = null;
        Configure();
    }

    protected abstract AnimationBuilder OnConfigure();
    protected abstract void OnAnimationTick((float current, float time) value);
    protected virtual void OnAnimationDisposed() {}
    protected virtual void OnAnimationCompleted() {}

    private void Configure()
        => _animation = OnConfigure()
            .SetLooped(_looped)
            .SetPlayOnce(_playOnce)
            .SetUnscaledTime(_unscaledTime)
            .OnUpdate(OnAnimationTick)
            .OnDisposed(OnAnimationDisposed)
            .OnComplete(OnAnimationCompleted)
            .Build();
}

