using System;
using System.Linq;
using UnityEngine;

public class Table : TouchableMonoBehaviour
{
    private Chair[] _chairs;

    [SerializeField] private Orientation _orientation;
    public Orientation Orientation => _orientation;

    public override void Awake()
    {
        base.Awake();
        
        CancelSelectionOnTouch = false;
        _chairs = GetComponentsInChildren<Chair>();
        CanSeat = _chairs.Length > 0;
    }

    public bool CanSeat { get; private set; }
    public Customer Customer { get; private set; }

    public void Seat(Customer customer, Chair chair = null)
    {
        chair ??= _chairs.First();
        var position = chair.transform.position + chair.Offset;
        customer.transform.position = position;
        Customer = customer;
        Customer.Table = this;
    }

    protected override void OnTouch()
    {
        if (Customer.IsDestroyed()) return;
        Customer.InvokeTouch(this, EventArgs.Empty);
    }

    public void ClearSeat()
    {
        Customer = null;
    }
}