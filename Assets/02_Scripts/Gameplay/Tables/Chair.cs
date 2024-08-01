using System;
using UnityEngine;

[RequireComponent(typeof(ScalingAnimator))]
public class Chair : Touchable
{
    [SerializeField] private Direction _side;
    private ScalingAnimator _animator;


    public Table Table { get; private set; }
    public Direction Side => _side;
    public ScalingAnimator Animator => _animator;

    public override void Awake()
    {
        base.Awake();
        CancelSelectionOnTouch = false;
        Table = GetComponentInParent<Table>()
            ?? throw new Exception($"The chair \"{gameObject.name}\" could not find a component {nameof(Table)} in its parent \"{gameObject.transform.parent.name}\".");

        _animator = this.GetRequiredComponent<ScalingAnimator>();
    }
}
