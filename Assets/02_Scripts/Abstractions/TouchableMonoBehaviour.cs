using System;
using UnityEngine;

// TODO: Refactor to be part of the selection handler
// Required so we recognize clicks in the void -> deselection
public class TouchableMonoBehaviour : MonoBehaviour
{
    private bool _touching;

    public bool CancelSelectionOnTouch { get; protected set; } = true;
    
    public event EventHandler Touch;

    public virtual void Awake()
    {
        var hitbox = GetComponent<Collider>();
        if (hitbox is null)
            throw new Exception($"The GameObject {gameObject.name} should be touchable but is missing a collider!");

        Touch += (_, _) => OnTouch();
    }

    protected virtual void OnTouch()
    {
        
    }

    public void InvokeTouch(object _, EventArgs eventArgs) 
        => Touch?.Invoke(this, eventArgs);
}