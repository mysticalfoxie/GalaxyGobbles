using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation2D
{
    private readonly Vector2 _start;
    private readonly Vector2 _end;
    private readonly Animation2DOptions _options;
    private readonly List<Vector2> _frames = new();
    private bool _cancelled;

    public Animation2D(Vector2 start, Vector2 end, Animation2DOptions options)
    {
        _start = start;
        _end = end;
        _options = options;
    }
    
    public event EventHandler Started;
    public event EventHandler<Vector2> Update;
    public event EventHandler Ended;
    public event EventHandler Cancelled;

    public IEnumerator Start()
    {
        Started?.Invoke(this, EventArgs.Empty);
        CalculateRoute();
        yield return StartInternal();
    }

    public void Cancel()
    {
        _cancelled = true;
        Cancelled?.Invoke(this, EventArgs.Empty);
    }

    private void CalculateRoute()
    {
        var vector = _end - _start;
        var count = _options.FrameCount; // Clone, so it doesnt change during enumeration
        var divisions = vector / count;
        var current = _start;
        
        for (var i = 0; i < count; i++)
        {
            _frames.Add(current);
            current += divisions;
        }

    }

    private IEnumerator StartInternal()
    {
        foreach (var frame in _frames)
        {
            yield return new CancellableWaitForSeconds(_options.FrameDelay, () => _cancelled);
            if (_cancelled) yield break;
            
            Update?.Invoke(this, frame);
        }
        
        Ended?.Invoke(this, EventArgs.Empty);
    }
}