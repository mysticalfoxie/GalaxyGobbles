using System;
using UnityEngine;

public class TouchExpandAnimator : ScalingAnimator
{
    private void OnEnable()
    {
        ResetScaleAfterComplete = false;
    }
    
    public Vector3 Original { get; set; }
    
    protected override AnimationBuilder OnConfigure()
        => AnimationBuilder 
            .CreateNew(transform.localScale.x, Original.x, _duration)
            .SetInterpolation(_interpolation);

    public void OnValidate()
    {
        if (Math.Abs(_strength - 1.0F) > 0.001)
        {
            Debug.LogWarning("[Touch Expand Animator] The strength is obsolete for this, because the animation always plays to the original scale.");
            Debug.LogWarning("[Touch Expand Animator] The strength is therefore configured in the shrink animation and influences this one.");
            _strength = 1.0F;
        }

        // ReSharper disable once InvertIf -- prefered style here.
        if (_weight != Vector3.one)
        {
            Debug.LogWarning("[Touch Expand Animator] The weight cannot be configured for this animator, because is aspect ratio dependent.");
            _weight = Vector3.one;
        }
    }

    protected override bool CustomAnimationTick((float current, float time) value)
    {
        if (!this || !transform || !isActiveAndEnabled)
        {
            Stop();
            return true;
        }
        
        // Start: 5, 10, 10
        // End: 10, 
        // 6 / 5 * 10

        var ratioDelta = value.current / Original.x;
        var y = Original.y * ratioDelta;
        var z = Original.z * ratioDelta;

        transform.localScale = new Vector3(value.current, y, z);
        return true;
    }
}

