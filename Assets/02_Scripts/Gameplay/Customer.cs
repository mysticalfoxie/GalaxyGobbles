using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Customer : SelectableMonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private Material _selectedStateMaterial;
    [SerializeField] private Sprite _thinkingBackgroundSprite;
    [SerializeField] private Sprite _thinkingSprite;
    [SerializeField] private Sprite _waitForCheckoutSprite;
    [SerializeField] private Sprite _waitForSeatSprite;

    internal List<ItemData> DesiredItems;

    private CustomerState _state;
    private CustomerData _data;
    private MeshRenderer _renderer;
    private Material _materialO;
    private SpriteRenderer _itemRenderer;
    private SpriteRenderer _itemBackgroundRenderer;
    
    public event EventHandler Leave;

    public CustomerData Data
    {
        get => _data;
        set => UpdateData(value);
    }

    public CustomerState State
    {
        get => _state;
        set => UpdateStatus(value);
    }

    public override void Awake()
    {
        _renderer = gameObject.GetComponent<MeshRenderer>();
        
        var itemRenderer = this.GetChildrenRecursively().Select(x => x.GetComponent<SpriteRenderer>()).ToArray();
        _itemRenderer = itemRenderer[0];
        _itemBackgroundRenderer = itemRenderer[1];

        State = CustomerState.WaitingForSeat;
        base.Awake();
    }

    protected override void OnTouch()
    {
        TryReceiveMeal();
        TryCheckout();
    }

    private void UpdateData(CustomerData data)
    {
        _data = data;
        DesiredItems = Data.DesiredItems.ToList();
    }

    private void UpdateStatus(CustomerState state)
    {
        _state = state;
        _itemRenderer.sprite = GetStatusSprite();
        _itemBackgroundRenderer.sprite = _itemRenderer.sprite is not null ? _thinkingBackgroundSprite : null;
    }

    private Sprite GetStatusSprite()
        => _state switch
        {
            CustomerState.WaitingForSeat => _waitForSeatSprite,
            CustomerState.WaitingForMeal => null, //DesiredItems.First().Sprites.First().Sprite,
            CustomerState.ThinkingAboutMeal => _thinkingSprite,
            CustomerState.WaitingForCheckout => _waitForCheckoutSprite,
            _ => null
        };

    public void TryCheckout()
    {
        if (State != CustomerState.WaitingForCheckout) return;
        Leave?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }

    public void TryReceiveMeal()
    {
        if (State != CustomerState.WaitingForMeal) return;
        if (!Sidebar.Instance.Inventory.HasItem(DesiredItems.First())) return;
        var item = Sidebar.Instance.Inventory.GetItemOfType(DesiredItems.First());
        Sidebar.Instance.Inventory.Remove(item);
        StartCoroutine(nameof(StartEating));
    }

    public IEnumerator StartEating()
    {
        State = CustomerState.Eating;
        _itemRenderer.sprite = null;
        yield return new WaitForSeconds(5);
        State = CustomerState.WaitingForCheckout;
    }

    public void OnSeated()
    {
        State = CustomerState.ThinkingAboutMeal;
        StartCoroutine(nameof(OnThinkingStart));
    }

    public IEnumerator OnThinkingStart()
    {
        yield return new WaitForSeconds(3);
        //_itemRenderer.sprite = DesiredItems.First().Sprites.First().Sprite; // TODO: Multi-Item-Support
        State = CustomerState.WaitingForMeal;
    }

    public override bool IsSelectable() => State == CustomerState.WaitingForSeat;

    protected override void OnSelected()
    {
        _materialO = _renderer.material;
        _renderer.material = _selectedStateMaterial;
    }

    protected override void OnDeselected()
    {
        _renderer.material = _materialO;
    }

    public static Customer Create(CustomerData data)
    {
        var customerGameObject = Instantiate(GameSettings.Data.PRE_Customer);
        var customer = customerGameObject.GetComponent<Customer>();
        customer.Data = data;
        return customer;
    }
}