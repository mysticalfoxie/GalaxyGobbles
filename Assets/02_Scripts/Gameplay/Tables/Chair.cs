using System;
using UnityEngine;

public class Chair : TouchableMonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    public Table Table { get; private set; }
    public Vector3 Offset => _offset;

    public override void Awake()
    {
        base.Awake();
        CancelSelectionOnTouch = false;
        Table = GetComponentInParent<Table>()
            ?? throw new Exception($"The chair \"{gameObject.name}\" could not find a component {nameof(Table)} in its parent \"{gameObject.transform.parent.name}\".");
    }
}