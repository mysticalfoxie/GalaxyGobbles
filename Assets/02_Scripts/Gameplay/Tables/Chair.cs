using System;
using UnityEngine;

[RequireComponent(typeof(ScalingAnimator))]
public class Chair : Touchable
{
    [SerializeField] private Direction _side;
    
    public Table Table { get; private set; }
    public Direction Side => _side;

    private ScalingAnimator _animator;
    private bool _playing;

    public override void Awake()
    {
        base.Awake();
        CancelSelectionOnTouch = false;
        Table = GetComponentInParent<Table>()
            ?? throw new Exception($"The chair \"{gameObject.name}\" could not find a component {nameof(Table)} in its parent \"{gameObject.transform.parent.name}\".");
        
        _animator = this.GetRequiredComponent<ScalingAnimator>();
    }

    protected override void OnPush()
    {
        if (!_playing) return;
        _animator.Stop();
    }

    protected override void OnRelease()
    {
        if (!_playing) return;
        _animator.Play();
    }

    public void StopAnimation()
    {
        if (!_playing) return;
        _playing = false;
        _animator.Stop();
    }

    public void StartAnimation()
    {
        if (_playing) return;
        _playing = true;
        _animator.Play();
    }
}
