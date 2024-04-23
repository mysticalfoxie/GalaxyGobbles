using System;
using UnityEngine.Serialization;
using UnityEngine;

// TODO: Refactor to be part of the selection handler
// Required so we recognize clicks in the void -> deselection
public class TouchableMonoBehaviour : MonoBehaviour
{
    private bool _touching;
    
    public event EventHandler Click;

    public virtual void Awake()
    {
        var hitbox = GetComponent<Collider>();
        if (hitbox is null)
            throw new Exception($"The GameObject {gameObject.name} should be touchable but is missing a collider!");

        Click += (_, _) => OnClick();
    }
    
    public virtual void OnClick()
    {
        
    }

    public void InvokeClick(object sender, EventArgs eventArgs) 
        => Click?.Invoke(sender, eventArgs);
}