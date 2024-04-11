using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Customer : TouchableMonoBehaviour, ISelectable
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

    public bool Selected { get; private set; }
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
        
        var itemRenderer = this.GetAllChildren().Select(x => x.GetComponent<SpriteRenderer>()).ToArray();
        _itemRenderer = itemRenderer[0];
        _itemBackgroundRenderer = itemRenderer[1];

        State = CustomerState.WaitingForSeat;
        base.Awake();
    }

    public override void OnClick()
    {
        HandleSelection();
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
            CustomerState.WaitingForMeal => DesiredItems.First().Sprite,
            CustomerState.ThinkingAboutMeal => _thinkingSprite,
            CustomerState.Eating => null,
            CustomerState.WaitingForCheckout => _waitForCheckoutSprite,
            _ => null
        };

    public void TryCheckout()
    {
        if (State != CustomerState.WaitingForCheckout) return;

        Destroy(gameObject);
    }

    public void TryReceiveMeal()
    {
        if (State != CustomerState.WaitingForMeal) return;
        if (!Inventory.Instance.HasItem(DesiredItems.First())) return;
        Inventory.Instance.Remove(DesiredItems.First());
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
        _itemRenderer.sprite = DesiredItems.First().Sprite; // TODO: Multi-Item-Support
        State = CustomerState.WaitingForMeal;
    }

    private void HandleSelection()
    {
        switch (Selected)
        {
            case true:
                Deselect();
                return;
            case false when IsSelectable():
                SelectionHandler.Instance.Select(this);
                break;
        }
    }

    private bool IsSelectable()
        => State switch
        {
            CustomerState.Eating => false,
            CustomerState.ThinkingAboutMeal => false,
            CustomerState.WaitingForCheckout => false,
            CustomerState.WaitingForMeal => false,
            CustomerState.WaitingForSeat => true,
            _ => false,
        };

    // TODO: Outsource to "SelectableMonoBehaviour"
    public void Select()
    {
        _materialO = _renderer.material;
        _renderer.material = _selectedStateMaterial;
        Selected = true;
    }

    public void Deselect()
    {
        _renderer.material = _materialO;
        Selected = false;
    }
}