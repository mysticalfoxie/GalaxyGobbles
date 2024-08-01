using UnityEngine;

public class PulseScalingAnimator : MonoBehaviour
{
    [Header("Animation")]
    [Tooltip("The duration of the chair pulsing animation.")]
    [SerializeField] [Range(0.1F, 10.0F)] private float _duration = 1.0F;
    [Tooltip("The scale increase and decrease as a factor.")]
    [SerializeField] [Range(0.1F, 10.0F)] private float _strength = 1.0F;
    [Tooltip("The multipliers for each axis. 1.0F = fully affected, 0.0F = static - not moving.")]
    [SerializeField] private Vector3 _weight = new(1.0F, 1.0F, 1.0F);

    [SerializeField] private bool _playOnAwake;
    [SerializeField] private bool _looped;

    public bool Playing => _animation is not null;
    
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

    private AnimationBuilder _animation;
    private Vector3 _original;

    public void Awake()
    {
        if (!_playOnAwake) return;
        StartPulsating();
    }
    
    public void StartPulsating()
    {
        if (_animation is not null) return;
        
        _original = transform.localScale;
        _animation = AnimationBuilder
            .CreateNew()
            .From(1)
            .To(_strength)
            .SetDuration(_duration)
            .SetInterpolation(AnimationInterpolation.Pulse)
            .OnUpdate(OnAnimationTick)
            .OnDisposed(OnAnimationDisposed)
            .SetLooped(_looped)
            .Build()
            .Start();
    }

    private void OnAnimationTick((float c, float t) value)
    {
        if (!this || !transform || !isActiveAndEnabled)
        {
            StopPulsating();
            return;
        }
        
        var strength = new Vector3(value.c, value.c, value.c);
        var scale = _original.Multiply(strength);
        var diff = scale.Subtract(_original);
        var weightedDiff = diff.Multiply(_weight);
        var newScale = _original.Add(weightedDiff);
        
        // Example 1: (Expanding)
        // Current: 10, 10, 10
        
        // Strength: 2
        // New Value: 20, 20, 20
        // Diff: +10, +10, +10
        // Weight: 1, 0.5, 0
        // Actual Diff: +10, +5, 0
        
        // Value: 20, 15, 10
        
        
        // Example 2: (Shrinking)
        // Current: 10, 10, 10
        
        // Strength: 0.25
        // New Value: 2.5, 2.5, 2.5
        // Diff: -7.5, -7.5, -7.5
        // Weight: 1, 0.5, 0
        // Actual Diff: -7.5, -3.75, 0
        
        // Value: 2.5, 6.25, 10

        transform.localScale = newScale;
    }

    public void OnAnimationDisposed()
    {
        if (!transform) return;
        transform.localScale = _original;
    }

    public void StopPulsating()
    {
        if (_animation is null) return;
        _animation.Stop();
        _animation = null;
    }
}