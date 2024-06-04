using System;
using UnityEngine;

public class CancellableWaitForSeconds : CustomYieldInstruction
{
    private readonly Func<bool> _cancelled;
    private float _time;

    public CancellableWaitForSeconds(float time, Func<bool> cancelled)
    {
        _time = time;
        _cancelled = cancelled;
    }

    public override bool keepWaiting => Tick();

    public bool Tick()
    {
        _time -= Time.deltaTime;
        var complete = _cancelled() || _time <= 0.0F; 
        return !complete;
    }
}