using System;
using System.Collections;
using UnityEngine;

public class Customer : SelectableMonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private Material _selectedStateMaterial;

    private CustomerData _data;
    private Material _materialO;

    public event EventHandler Leave;
    public event EventHandler DataChange;

    public CustomerStateMachine StateMachine { get; private set; }
    public MeshRenderer MeshRenderer { get; private set; }

    public CustomerData Data
    {
        get => _data;
        set => UpdateData(value);
    }

    public Table Table { get; set; }

    public override void Awake()
    {
        base.Awake();
        
        MeshRenderer = this.GetRequiredComponent<MeshRenderer>();
        StateMachine = this.GetRequiredComponent<CustomerStateMachine>();
    }


    protected override void OnTouch()
    {
        if (TryReceiveMeal()) return;
        TryCheckout();
        // TODO: Play sound uwu if you click them without needing to click them ;p ;p ;p
    }

    private void UpdateData(CustomerData data)
    {
        _data = data;
        DataChange?.Invoke(this, EventArgs.Empty);
    }

    public bool TryCheckout()
    {
        if (StateMachine.State != CustomerState.WaitingForCheckout) return false;
        StateMachine.State = CustomerState.Leaving;
        Leave?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
        return true;
    }

    public bool TryReceiveMeal()
    {
        if (StateMachine.State != CustomerState.WaitingForMeal) return false;
        StartCoroutine(nameof(StartEating));
        return true;
    }

    public IEnumerator StartEating()
    {
        StateMachine.State = CustomerState.Eating;
        yield return new WaitForSeconds(GameSettings.Data.CustomerEatingTime);
        StateMachine.State = CustomerState.WaitingForCheckout;
    }

    public void OnSeated()
    {
        StateMachine.State = CustomerState.ThinkingAboutMeal;
        StartCoroutine(nameof(OnThinkingStart));
    }

    public IEnumerator OnThinkingStart()
    {
        yield return new WaitForSeconds(GameSettings.Data.CustomerThinkingTime);
        StateMachine.State = CustomerState.WaitingForMeal;
    }


    public override bool IsSelectable() => StateMachine.State == CustomerState.WaitingForSeat;

    protected override void OnSelected()
    {
        _materialO = MeshRenderer.material;
        MeshRenderer.material = _selectedStateMaterial;
    }

    protected override void OnDeselected()
    {
        MeshRenderer.material = _materialO;
    }

    public static Customer Create(CustomerData data)
    {
        var customerGameObject = Instantiate(GameSettings.Data.PRE_Customer);
        var customer = customerGameObject.GetComponent<Customer>();
        customer.Data = data;
        return customer;
    }
}