using System;
using UnityEngine;

public class Touchable : MonoBehaviour
{
    private bool _touching;

    public bool CancelSelectionOnTouch { get; protected set; } = true;
    
    public event EventHandler Touch;
    public event EventHandler Push;
    public event EventHandler Release;

    public virtual void Awake()
    {
        Touch += (_, _) => OnTouch();
        Push += (_, _) => OnPush();
        Release += (_, _) => OnRelease();
    }

    protected virtual void OnTouch() { }
    protected virtual void OnPush() { }
    protected virtual void OnRelease() { }

    public void InvokeTouch(object _, EventArgs eventArgs) => Touch?.Invoke(this, eventArgs);
    public void InvokePush(object sender, EventArgs eventArgs) => Push?.Invoke(sender, eventArgs);
    public void InvokeRelease(object sender, EventArgs eventArgs) => Release?.Invoke(sender, eventArgs);
}