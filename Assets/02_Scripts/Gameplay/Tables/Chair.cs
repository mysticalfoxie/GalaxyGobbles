using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Chair : Touchable
{
    [FormerlySerializedAs("_offset")] [SerializeField] private Vector3 _customerOffset;
    [SerializeField] private Direction _heartDirection;
    
    public Table Table { get; private set; }
    public Vector3 CustomerOffset => _customerOffset;
    public Direction Direction => _heartDirection;

    public override void Awake()
    {
        base.Awake();
        CancelSelectionOnTouch = false;
        Table = GetComponentInParent<Table>()
            ?? throw new Exception($"The chair \"{gameObject.name}\" could not find a component {nameof(Table)} in its parent \"{gameObject.transform.parent.name}\".");
    }
}