using System;
using UnityEngine;

public class Chair : Touchable
{
    [SerializeField] private Direction _side;
    private AnimationBuilder _animation;

    public Table Table { get; private set; }
    public Direction Side => _side;

    public override void Awake()
    {
        base.Awake();
        CancelSelectionOnTouch = false;
        Table = GetComponentInParent<Table>()
            ?? throw new Exception($"The chair \"{gameObject.name}\" could not find a component {nameof(Table)} in its parent \"{gameObject.transform.parent.name}\".");

        StartPulsating();
    }

    public void StartPulsating()
    {
        _animation = AnimationBuilder
            .CreateNew()
            .From(transform.localScale.x)
            .To(transform.localScale.x * GameSettings.Data.ChairPulsateStrength)
            .SetDuration(GameSettings.Data.ChairPulsateDuration)
            .SetInterpolation(AnimationInterpolation.Pulse)
            .OnUpdate(x => transform.localScale = transform.localScale.Multiply(x.c))
            .SetLooped()
            .Build()
            .Start();
    }

    public void StopPulsating()
    {
        _animation.Stop();
    }
}
