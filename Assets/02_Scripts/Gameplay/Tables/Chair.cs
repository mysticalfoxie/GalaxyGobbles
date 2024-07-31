using System;
using System.Collections;
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

        StartCoroutine(Foo());
        return;

        IEnumerator Foo()
        {
            yield return new WaitForSeconds(5000);
            StopPulsating();
            yield return new WaitForSeconds(3000);
            StartPulsating();
            yield return new WaitForSeconds(5000);
            StopPulsating();
        }
    }

    public void StartPulsating()
    {
        if (_animation is not null) return;
        
        var original = transform.localScale;
        _animation = AnimationBuilder
            .CreateNew()
            .From(1)
            .To(GameSettings.Data.ChairPulsateStrength)
            .SetDuration(GameSettings.Data.ChairPulsateDuration)
            .SetInterpolation(AnimationInterpolation.Pulse)
            .OnUpdate(x => transform.localScale = original.Multiply(x.c))
            .OnDisposed(() => transform.localScale = original)
            .SetLooped()
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
