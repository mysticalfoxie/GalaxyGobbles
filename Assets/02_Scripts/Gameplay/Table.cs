using UnityEngine;

public class Table : TouchableMonoBehaviour
{
    [Header("Table Properties")]
    [SerializeField]
    private Transform _customerPosition;
    
    public Customer SeatedCustomer { get; private set; }

    public void Start()
    {
        SelectionHandler.Instance.Register(this);
    }

    public void Seat(Customer customer)
    {
        customer.transform.position = _customerPosition.position;
        SeatedCustomer = customer;
        SeatedCustomer.OnSeated();
    }

    public void ClearSeat()
    {
        SeatedCustomer = null;
    }

    public override void OnClick()
    {
        if (SeatedCustomer is not null)
            SeatedCustomer.TryReceiveMeal();
    }
}