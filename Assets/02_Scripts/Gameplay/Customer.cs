using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Customer : TouchableMonoBehaviour, ISelectable
{
    [SerializeField] private Material _selectedStateMaterial;

    internal List<ItemData> DesiredItems;
    internal CustomerState State;
    private CustomerData _data;
    private MeshRenderer _renderer;
    private Material _materialO;
    private SpriteRenderer _itemRenderer;

    public bool Selected { get; private set; }
    public CustomerData Data
    {
        get => _data;
        set => UpdateData(value);
    }

    public override void Awake()
    {
        _renderer = gameObject.GetComponent<MeshRenderer>();
        _itemRenderer = this
            .GetChildren()
            .Select(x => x.GetComponent<SpriteRenderer>())
            .First(x => x != null);

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

    private void TryCheckout()
    {
        if (State != CustomerState.WaitingForCheckout) return;

        Destroy(this);
    }

    public void TryReceiveMeal()
    {
        if (State != CustomerState.WaitingForMeal) return;
        if (!Inventory.Instance.HasItem(DesiredItems.First())) return;

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