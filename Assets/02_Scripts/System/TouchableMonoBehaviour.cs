using System;
using UnityEngine;
using UnityEngine.Serialization;

public class TouchableMonoBehaviour : MonoBehaviour
{
    private bool _touching;
    
    [FormerlySerializedAs("_collider")]
    [Header("Touch Support")]
    [SerializeField]
    private Collider _raycastHitbox;

    [SerializeField] 
    [Range(1, 300)]
    private float _raycastDistance = 10.0F; 

    public virtual void Awake()
    {
        _raycastHitbox = GetComponent<Collider>();
        if (_raycastHitbox is null)
            throw new Exception("A clickable object has to have a collider!");
    }

    public virtual void Update()
    {
        ExecTouchDetection();
    }

    private void ExecTouchDetection()
    {
        if (Input.GetMouseButtonDown(default) && IsTouchingThisGameObject())
            _touching = true;
        
        if (!_touching || !Input.GetMouseButtonUp(default)) 
            return;
        
        _touching = false;
        
        if (IsTouchingThisGameObject())
            OnClick();
    }

    private bool IsTouchingThisGameObject()
    {
        var ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out var hit, _raycastDistance) 
               && hit.transform is not null
               && hit.collider == _raycastHitbox;
    }
    
    public virtual void OnClick()
    {
        
    }
}