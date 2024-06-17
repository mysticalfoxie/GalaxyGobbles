using System;
using UnityEngine;

public class Touchable : MonoBehaviour
{
    private bool _touching;

    public bool CancelSelectionOnTouch { get; protected set; } = true;
    
    public event EventHandler Touch;

    public virtual void Awake()
    {
        Touch += (_, _) => OnTouch();
    }

    protected virtual void OnTouch()
    {
        
    }

    public void InvokeTouch(object _, EventArgs eventArgs) 
        => Touch?.Invoke(this, eventArgs); 
}