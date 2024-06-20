using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CustomerStateRenderer))]
[RequireComponent(typeof(CustomerStateMachine))]
[RequireComponent(typeof(Patience))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SpriteRenderer))]
public class Customer : Selectable
{
    private static GameObject _customerRoot;
    private CustomerData _data;
    private bool _poisoned;
    private bool _dying;
    private bool _visible;
    private bool _selected;
    private bool _orderedTwice;

    public event EventHandler Destroying;

    public Patience Patience { get; private set; }
    public CustomerStateMachine StateMachine { get; private set; }
    public CustomerStateRenderer Renderer { get; private set; }
    public List<ItemData> DesiredItems { get; } = new();
    public Table Table { get; set; }
    public Chair Chair { get; set; }

    public bool Visible
    {
        get => _visible;
        set => UpdateVisibility(value);
    }

    public CustomerData Data
    {
        get => _data;
        set => UpdateData(value);
    }

    public override void Awake()
    {
        base.Awake();

        Renderer = this.GetRequiredComponent<CustomerStateRenderer>();
        StateMachine = this.GetRequiredComponent<CustomerStateMachine>();
        Patience = this.GetRequiredComponent<Patience>();
        Destroying += (_, _) => StateMachine.Renderer.Dispose();
        Destroying += (_, _) => Patience.Dispose();
        Patience.Customer = this;
        Patience.Angry += OnAngry;
    }

    public void InitializeInEditorMode()
    {
        Renderer = this.GetRequiredComponent<CustomerStateRenderer>();
        StateMachine = this.GetRequiredComponent<CustomerStateMachine>();
        Patience = this.GetRequiredComponent<Patience>();
        Renderer.InitializeInEditorMode();
    }

    public void TryCheckout()
    {
        if (StateMachine.State != CustomerState.WaitingForCheckout) return;
        StateMachine.State = CustomerState.Leaving;

        new ScoreCalculation(Patience.Value, _data.DesiredItems).Apply();

        OnCustomerLeave();
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
        if (HandlePoisoned()) yield break;
        if (HandleOrderingTwice()) yield break;
        if (HandleOrderingSake()) yield break;

        StateMachine.State = CustomerState.WaitingForCheckout;
    }

    public void OnSeated()
    {
        Renderer.SetSeated();
        Renderer.RenderSeated();
        Patience.UpdateOffset();
        StateMachine.State = CustomerState.ThinkingAboutMeal;
        StartCoroutine(nameof(OnThinkingStart));
    }

    public IEnumerator OnThinkingStart()
    {
        yield return new CancellableWaitForSeconds(GameSettings.Data.CustomerThinkingTime, () => _dying);
        if (_dying) yield break;

        var doesNotOrder = Random.Range(1, 100) <= Data.Species.ChanceToNotOrder;
        StateMachine.State = doesNotOrder ? CustomerState.Leaving : CustomerState.WaitingForMeal;
    }

    public override bool IsSelectable() => StateMachine.State == CustomerState.WaitingForSeat;

    public void Kill()
    {
        StateMachine.State = CustomerState.Dying;
        StartCoroutine(nameof(StartDying));
    }

    protected override void OnSelected()
    {
        _selected = true;
        Renderer.OnSelected();
        var operation = SelectionSystem.Instance.WaitForTableSelection(OnTableSelected, () => !_selected);
        StartCoroutine(operation);
    }

    private static void OnTableSelected(TableSelectEvent eventArgs)
    {
        if (eventArgs?.Table is null) return;
        Tables.OnTableSelected(eventArgs.Table, eventArgs.Chair);
    }

    protected override void OnDeselected()
    {
        _selected = false;
        Renderer.OnDeselected();
    }

    protected override void OnTouch()
    {
        if (TryReceiveMeal()) return;
        TryCheckout();
    }

    private IEnumerator StartDying()
    {
        yield return new WaitForSeconds(GameSettings.Data.CustomerDyingTime);
        OnCustomerDied();
    }

    private IEnumerator StartAngryLeaving()
    {
        yield return new WaitForSeconds(GameSettings.Data.CustomerAngryLeavingTime);
        OnCustomerLeave();
    }

    private void OnCustomerLeave()
    {
        WaitAreaHandler.Instance.RemoveCustomer(this);
        Destroying?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }

    private void OnCustomerDied()
    {
        var bounty = Model.Create<BountyData>(model =>
        {
            model.WasTarget = _data._isAssassinationTarget;
            model.Species = _data.Species;
        });

        if (!BottomBar.Instance.Bounties.TryAdd(bounty))
            Debug.Log("[Bounties] Cannot pick up any more bounties.");

        OnCustomerLeave();
    }

    private bool HandleOrderingSake()
    {
        if (Random.Range(1, 100) > Data.Species.ChanceToOrderSake) return false;

        DesiredItems.Add(GameSettings.GetItemMatch(Identifiers.Value.Sake));
        StateMachine.State = CustomerState.ThinkingAboutMeal;
        StartCoroutine(nameof(OnThinkingStart));

        return true;
    }

    private bool HandlePoisoned()
    {
        if (!_poisoned) return false;
        StateMachine.State = CustomerState.Poisoned;
        return true;
    }

    private bool HandleOrderingTwice()
    {
        if (_orderedTwice) return false;
        if (Random.Range(1, 100) > Data.Species.ChanceToOrderTwice) return false;
        _orderedTwice = true;

        DesiredItems.AddRange(Data.DesiredItems);
        StateMachine.State = CustomerState.ThinkingAboutMeal;
        StartCoroutine(nameof(OnThinkingStart));

        return true;
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

    private void UpdateData(CustomerData data)
    {
        _data = data;

        DesiredItems.Clear();
        DesiredItems.AddRange(_data.DesiredItems);

        if (!Renderer) return;
        Renderer.OnCustomerDataSet();
    }

    private void UpdateVisibility(bool value)
    {
        _visible = value;

        if (!_visible) return;
        if (Patience.Ticking) return;

        Patience.StartTicking();
    }

    private void OnItemReceived(ItemData desiredItemData, Item inventoryItem)
    {
        DesiredItems.Remove(desiredItemData);
        if (inventoryItem.Data.Poison is null) return;
        if (_data.Species.PoisonItems.All(x => x.name != inventoryItem.Data.Poison.name)) return;
        _poisoned = true;
    }

    private void OnAngry(object sender, EventArgs eventArgs)
    {
        if (_dying) return;
        StateMachine.State = CustomerState.Angry;
        StartCoroutine(nameof(StartAngryLeaving));
    }

    public static Customer Create(CustomerData data)
    {
        if (!_customerRoot.IsAssigned()) _customerRoot = null;
        _customerRoot ??= GameObject.Find("Customers") ?? References.Instance.RootObject;
        var customerGameObject = Instantiate(GameSettings.Data.PRE_Customer);
        var customer = customerGameObject.GetComponent<Customer>();
        customerGameObject.transform!.SetParent(_customerRoot.transform);
        customer.Data = data;
        return customer;
    }
}