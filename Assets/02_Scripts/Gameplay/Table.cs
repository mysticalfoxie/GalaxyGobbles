using UnityEngine;

public class Table : TouchableMonoBehaviour
{
    [Header("Table Properties")]
    [SerializeField]
    private Transform _customerPosition;

    public override void Awake()
    {
        base.Awake();
        
        CancelSelectionOnTouch = false;
    }

    public Customer SeatedCustomer { get; private set; }

    public void Seat(Customer customer)
    {
        customer.transform.position = _customerPosition.position;
        SeatedCustomer = customer;
    }

    public void ClearSeat()
    {
        SeatedCustomer = null;
    }
}