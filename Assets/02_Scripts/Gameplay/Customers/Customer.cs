using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        KittyBot.Instance.MoveTo(Table.transform, OnCheckout);
    }

    private void OnCheckout()
    {
        StateMachine.State = CustomerState.Leaving;
        new ScoreCalculation(Patience.Value, _data.DesiredItems).Apply();
        AudioManager.Instance.PlaySFX(AudioSettings.Data.MoneyPaid);
        OnCustomerLeave();
    }

    public bool TryReceiveMeal()
    {
        if (StateMachine.State != CustomerState.WaitingForMeal) return false;
        if (!HasItemMatch()) return false;

        KittyBot.Instance.MoveTo(Table.transform, OnReceiveMeal);

        return true;
    }

    private void OnReceiveMeal()
    {
        ReceiveItemsFromInventory();
        if (DesiredItems.Count == 0)
        {
            StartCoroutine(nameof(StartEating));
            return;
        }

        StateMachine.Renderer.RefreshDesiredItems();
    }

    public IEnumerator StartEating()
    {
        StateMachine.State = CustomerState.Eating;
        AudioManager.Instance.PlaySFX(AudioSettings.Data.EatingSound);
        yield return new CancellableWaitForSeconds(GameSettings.Data.CustomerEatingTime, () => _dying);
        if (_dying) yield break;
        if (HandlePoisoned()) yield break;
        // if (HandleOrderingTwice()) yield break;
        // if (HandleOrderingSake()) yield break;

        StateMachine.State = CustomerState.WaitingForCheckout;
    }

    public void OnSeated()
    {
        AudioManager.Instance.PlaySFX(AudioSettings.Data.CustomerBeaming);
        StartCoroutine(BeamToChair());
    }

    public IEnumerator OnThinkingStart()
    {
        yield return new CancellableWaitForSeconds(GameSettings.Data.CustomerThinkingTime, () => _dying);
        if (_dying) yield break;

        StateMachine.State = CustomerState.WaitingForMeal;
        AudioManager.Instance.PlaySFX(GetCustomerVoice());
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
        AudioManager.Instance.PlaySFX(AudioSettings.Data.Click);
        AudioManager.Instance.PlaySFX(GetCustomerVoice());
        var operation = SelectionSystem.Instance.WaitForTableSelection(OnTableSelected, () => !_selected);
        StartCoroutine(operation);
    }

    private static void OnTableSelected(TableSelectEvent eventArgs)
    {
        if (eventArgs?.Table is null)
        {
            AudioManager.Instance.PlaySFX(AudioSettings.Data.ErrorNah);
            return;
        }
        
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
        AudioManager.Instance.PlaySFX(GetCustomerAngryVoice());
        AudioManager.Instance.PlayMusic(AudioSettings.Data.TensionMusic, false);
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

    private AudioData GetCustomerVoice()
        => Patience.State switch
        {
            PatienceCategory.Love => GetCustomerLoveVoice(),
            PatienceCategory.Happy => GetCustomerHappyVoice(),
            PatienceCategory.Angry => GetCustomerAngryVoice(),
            _ => throw new ArgumentOutOfRangeException()
        };

    private AudioData GetCustomerAngryVoice()
    {
        if (Data.Species.name == Identifiers.Value.Ikaruz.name)
            return AudioSettings.Data.IcarusVoiceAngry;
        if (Data.Species.name == Identifiers.Value.Bob.name)
            return AudioSettings.Data.BobVoiceAngry;
        if (Data.Species.name == Identifiers.Value.Broccoloid.name)
            return AudioSettings.Data.BrocoloidVoiceAngry;
        throw new NotSupportedException($"[Leave Audio] Could not match the species {Data.Species.name} to the preserved species.");
    }

    private AudioData GetCustomerHappyVoice()
    {
        if (Data.Species.name == Identifiers.Value.Ikaruz.name)
            return AudioSettings.Data.IcarusVoiceHappy;
        if (Data.Species.name == Identifiers.Value.Bob.name)
            return AudioSettings.Data.BobVoiceHappy;
        if (Data.Species.name == Identifiers.Value.Broccoloid.name)
            return AudioSettings.Data.BrocoloidVoiceHappy;
        throw new NotSupportedException($"[Leave Audio] Could not match the species {Data.Species.name} to the preserved species.");
    }

    private AudioData GetCustomerLoveVoice()
    {
        if (Data.Species.name == Identifiers.Value.Ikaruz.name)
            return AudioSettings.Data.IcarusVoiceLove;
        if (Data.Species.name == Identifiers.Value.Bob.name)
            return AudioSettings.Data.BobVoiceLove;
        if (Data.Species.name == Identifiers.Value.Broccoloid.name)
            return AudioSettings.Data.BrocoloidVoiceLove;
        throw new NotSupportedException($"[Leave Audio] Could not match the species {Data.Species.name} to the preserved species.");
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

    // private bool HandleOrderingSake()
    // {
    //     if (Random.Range(1, 100) > Data.Species.ChanceToOrderSake) return false;
    //
    //     DesiredItems.Add(GameSettings.GetItemMatch(Identifiers.Value.Sake));
    //     StateMachine.State = CustomerState.ThinkingAboutMeal;
    //     StartCoroutine(nameof(OnThinkingStart));
    //
    //     return true;
    // }

    private bool HandlePoisoned()
    {
        if (!_poisoned) return false;
        StateMachine.State = CustomerState.Poisoned;
        StartCoroutine(nameof(StartPoisonCloudAnimation));
        return true;
    }

    private IEnumerator StartPoisonCloudAnimation()
    {
        yield return new WaitForSeconds(GameSettings.Data.CustomerKillDelay);

        CustomerPoisonRenderer.Instance.PoisonHidden += OnPoisonHidden;
        CustomerPoisonRenderer.Instance.MovingEnded += OnMovingEnded;
        CustomerPoisonRenderer.Instance.StartPoisonAnimation(this);
        AudioManager.Instance.PlaySFX(AudioSettings.Data.FartNoise);
        yield break;

        void OnMovingEnded(object sender, EventArgs e)
        {
            CustomerPoisonRenderer.Instance.MovingEnded -= OnMovingEnded;

            var target = Table.NeighbourTable.Customer;
            if (target is not null) target.Kill();
        }

        void OnPoisonHidden(object sender, EventArgs e)
        {
            CustomerPoisonRenderer.Instance.PoisonHidden -= OnPoisonHidden;
            Renderer.OnPoisonHidden();
            StateMachine.State = CustomerState.WaitingForCheckout;
        }
    }

    // private bool HandleOrderingTwice()
    // {
    //     if (_orderedTwice) return false;
    //     if (Random.Range(1, 100) > Data.Species.ChanceToOrderTwice) return false;
    //     _orderedTwice = true;
    //
    //     DesiredItems.AddRange(Data.DesiredItems);
    //     StateMachine.State = CustomerState.ThinkingAboutMeal;
    //     StartCoroutine(nameof(OnThinkingStart));
    //
    //     return true;
    // }

    private void ReceiveItemsFromInventory()
    {
        var list = BottomBar.Instance.Inventory.Items.ToArray();
        foreach (var item in list)
        {
            var match = DesiredItems.FirstOrDefault(x => x.name == item.Data.name);
            if (match is null) continue;

            OnItemReceived(match, item);
            BottomBar.Instance.Inventory.Remove(item, true);
        }
    }

    private bool HasItemMatch()
    {
        var list = BottomBar.Instance.Inventory.Items.ToArray();
        return list.Any(x => DesiredItems.Any(y => y.name == x.Data.name));
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

    private IEnumerator BeamToChair()
    {
        yield return new WaitForSeconds(GameSettings.Data.CustomerBeamingTime);
        AudioManager.Instance.PlaySFX(AudioSettings.Data.PuffChairDrop);
        Renderer.SetSeated();
        Renderer.RenderSeated();
        Patience.Add(GameSettings.Data.PatienceRegainOnSeated);
        StateMachine.State = CustomerState.ThinkingAboutMeal;
        StartCoroutine(nameof(OnThinkingStart));
    }

    private void OnItemReceived(ItemData desiredItemData, Item inventoryItem)
    {
        Patience.Add(GameSettings.Data.PatienceRegainOnItemReceive);
        AudioManager.Instance.PlaySFX(GetCustomerVoice());
        DesiredItems.Remove(desiredItemData);
        if (inventoryItem.Data.Poison is null) return;
        if (_data.Species.PoisonItems.All(x => x.name != inventoryItem.Data.Poison.name)) return;
        _poisoned = true;
    }

    private void OnAngry(object sender, EventArgs eventArgs)
    {
        if (_dying) return;
        StateMachine.State = CustomerState.Angry;
        AudioManager.Instance.PlaySFX(GetCustomerVoice());
        StartCoroutine(nameof(StartAngryLeaving));
    }

    public static Customer Create(CustomerData data)
    {
        if (!_customerRoot) _customerRoot = null;
        _customerRoot ??= GameObject.Find("Customers") ?? References.Instance.RootObject;
        var customerGameObject = Instantiate(GameSettings.Data.PRE_Customer);
        var customer = customerGameObject.GetComponent<Customer>();
        var scale = customerGameObject.transform.localScale;
        customerGameObject.transform!.SetParent(_customerRoot.transform);
        customerGameObject.transform.localScale = scale;
        customer.Data = data;
        return customer;
    }
}