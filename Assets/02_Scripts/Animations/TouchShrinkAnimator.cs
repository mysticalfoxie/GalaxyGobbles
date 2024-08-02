using UnityEngine;

public class TouchShrinkAnimator : ScalingAnimator
{
    public Vector3 Original { get; set; }

    private void OnEnable()
    {
        ResetScaleAfterComplete = false;
    }

    public override void Play()
    {
        transform.localScale = Original;
        base.Play();
    }

    protected override AnimationBuilder OnConfigure()
        =>  AnimationBuilder
            .CreateNew()
            .From(transform.localScale.x)
            .To(Original.x * (_strength > 1 ? 1 - _strength + 1 : _strength))
            .SetDuration(_duration)
            .SetInterpolation(_interpolation);

    public void OnValidate()
    {
        // ReSharper disable once InvertIf -- prefered style here.
        if (_weight != Vector3.one)
        {
            Debug.LogWarning("[Touch Shrink Animator] The weight cannot be configured for this animator, because is aspect ratio dependent.");
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
