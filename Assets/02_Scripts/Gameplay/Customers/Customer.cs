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
    private bool _dying;

    public event EventHandler Destroying;

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
        Destroying += (_, _) => StateMachine.Renderer.Dispose();
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

    public void TryCheckout()
    {
        if (StateMachine.State != CustomerState.WaitingForCheckout) return;
        StateMachine.State = CustomerState.Leaving;
        Destroying?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
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
        yield return new CancellableWaitForSeconds(GameSettings.Data.CustomerEatingTime, () => _dying);
        if (_dying) yield break;
        
        StateMachine.State = _poisoned ? CustomerState.Poisoned : CustomerState.WaitingForCheckout;
    }

    public void OnSeated()
    {
        StateMachine.State = CustomerState.ThinkingAboutMeal;
        StartCoroutine(nameof(OnThinkingStart));
    }

    public IEnumerator OnThinkingStart()
    {
        yield return new CancellableWaitForSeconds(GameSettings.Data.CustomerThinkingTime, () => _dying);
        if (_dying) yield break;
        
        StateMachine.State = CustomerState.WaitingForMeal;
    }

    private void ReceiveItemsFromInventory()
    {
        foreach (var item in BottomBar.Instance.Inventory.Items.ToArray())
        {
            var match = DesiredItems.FirstOrDefault(x => x.name == item.Data.name);
            if (match is null) continue;

            OnItemReceived(match, item);
            BottomBar.Instance.Inventory.Remove(item, true);
            return;
        }
    }

    private void OnItemReceived(ItemData desiredItemData, Item inventoryItem)
    {
        DesiredItems.Remove(desiredItemData);
        if (inventoryItem.Data.Poison is null) return;
        if (_data.Species.PoisonItems.All(x => x.name != inventoryItem.Data.Poison.name)) return;
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

    public void Kill()
    {
        StateMachine.State = CustomerState.Dying;
        StartCoroutine(nameof(StartDying));
    }

    public IEnumerator StartDying()
    {
        yield return new WaitForSeconds(GameSettings.Data.CustomerDyingTime);
        OnCustomerDied();
    }

    public void OnCustomerDied()
    {
        var bounty = Model.Create<BountyData>(model =>
        {
            model.WasTarget = _data._isAssassinationTarget;
            model.Species = _data.Species;
        });

        if (!BottomBar.Instance.Bounties.TryAdd(bounty)) 
            Debug.Log("[Bounties] Cannot pick up any more bounties.");
        
        Destroying?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }
}