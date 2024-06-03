using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Customer : SelectableMonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private Material _selectedStateMaterial;

    private CustomerData _data;
    private Material _materialO;
    private bool _poisoned;

    public event EventHandler Leave;

    public CustomerStateMachine StateMachine { get; private set; }
    public MeshRenderer MeshRenderer { get; private set; }
    public List<ItemData> DesiredItems { get; } = new();

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
    }

    private void UpdateData(CustomerData data)
    {
        _data = data;
        
        DesiredItems.Clear();
        DesiredItems.AddRange(_data.DesiredItems);
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
        ReceiveItemsFromInventory();
        if (DesiredItems.Count == 0)
        {
            StartCoroutine(nameof(StartEating));
            return true;
        }
        
        StateMachine.Renderer.RefreshDesiredItems();
        return true;
    }

    public IEnumerator StartEating()
    {
        StateMachine.State = CustomerState.Eating;
        yield return new WaitForSeconds(GameSettings.Data.CustomerEatingTime);
        StateMachine.State = _poisoned ? CustomerState.Poisoned : CustomerState.WaitingForCheckout;
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

    private void ReceiveItemsFromInventory()
    {
        foreach (var item in DesiredItems.ToArray())
            if (BottomBar.Instance.Inventory.TryRemove(item, true))
                OnItemReceived(item);
    }

    private void OnItemReceived(ItemData item)
    {
        DesiredItems.Remove(item);
        if (item.Poison is null) return;
        if (_data.Species.PoisonItems.All(x => x.name != item.Poison.name)) return;
        _poisoned = true;
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