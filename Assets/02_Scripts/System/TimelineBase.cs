using System;
using System.Collections;
using UnityEngine;

public abstract class TimelineBase : MonoBehaviour
{
    public event EventHandler Tick;
    private bool _destroyed;
    
    protected int Ticks { get; private set; }
    
    
    public void Start()
    {
        StartCoroutine(nameof(TimelineTick));
    }

    public IEnumerator TimelineTick()
    {
        while (!_destroyed)
        {
            yield return new WaitForSeconds(1);
            Tick?.Invoke(this, EventArgs.Empty);
            Ticks++;
        }
    }

    public void OnDestroy()
    {
        _destroyed = true;
    }
}