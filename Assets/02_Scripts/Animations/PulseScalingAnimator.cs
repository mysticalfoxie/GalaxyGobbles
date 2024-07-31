using System.Collections;
using UnityEngine;

public class PulseScalingAnimator : MonoBehaviour
{
    [Header("Animation")]
    [Tooltip("The scale increase and decrease as a factor.")]
    [SerializeField] [Range(0.1F, 10.0F)] private float _strength = 1.0F;
    [Tooltip("The duration of the chair pulsing animation.")]
    [SerializeField] [Range(0.1F, 10.0F)] private float _duration = 1.0F;

    [SerializeField] private bool _playOnAwake;
    [SerializeField] private bool _looped;
    
    private AnimationBuilder _animation;
    
    public void Awake()
    {
        if (!_playOnAwake) return;
        StartPulsating();
    }
    
    public void StartPulsating()
    {
        if (_animation is not null) return;
        
        var original = transform.localScale;
        _animation = AnimationBuilder
            .CreateNew()
            .From(1)
            .To(_strength)
            .SetDuration(_duration)
            .SetInterpolation(AnimationInterpolation.Pulse)
            .OnUpdate(x => transform.localScale = original.Multiply(x.c))
            .OnDisposed(() => transform.localScale = original)
            .SetLooped(_looped)
            .Build()
            .Start();
    }

    public void StopPulsating()
    {
        if (_animation is null) return;
        _animation.Stop();
        _animation = null;
    }
}