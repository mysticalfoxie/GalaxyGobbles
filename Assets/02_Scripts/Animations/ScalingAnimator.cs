using UnityEngine;

public class ScalingAnimator : AnimatorBase
{
    private Vector3 _original;
    private bool _played;

    protected bool ResetScaleAfterComplete { get; set; } = true;

    [Header("Scaling Animator")]
    [SerializeField] protected AnimationInterpolation _interpolation = AnimationInterpolation.Pulse;
    public override void Play()
    {
        base.Play();
        _original = transform.localScale;
        _played = true;
    }

    protected override AnimationBuilder OnConfigure()
        => AnimationBuilder
            .CreateNew(1, _strength, _duration)
            .SetInterpolation(_interpolation);

    protected override void OnAnimationTick((float current, float time) value)
    {
        if (CustomAnimationTick(value)) return;
        if (!this || !transform || !isActiveAndEnabled)
        {
            Stop();
            return;
        }
        
        var strength = new Vector3(value.current, value.current, value.current);
        var scale = _original.Multiply(strength);
        var diff = scale.Subtract(_original);
        var weightedDiff = diff.Multiply(_weight);
        var newScale = _original.Add(weightedDiff);

        transform.localScale = newScale;
    }

    protected virtual bool CustomAnimationTick((float current, float time) value)
    {
        return false;
    }
    
    protected override void OnAnimationDisposed()
    {
        if (!this) return;
        if (!ResetScaleAfterComplete) return;
        if (!transform) return;
        if (!_played) return;
        transform.localScale = _original;
    }
}

#region    Weighting Calculation Documentation
// --- Weighting Calculation ---
        
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
#endregion
