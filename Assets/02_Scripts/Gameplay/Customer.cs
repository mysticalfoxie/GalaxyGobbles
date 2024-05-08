using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Customer : SelectableMonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private Material _selectedStateMaterial;
    
    [Header("Item Positioning")]
    [SerializeField] private Vector2 _thinkBubbleOffset;
    [SerializeField] private Vector2 _thinkBubbleItemOffset;
    [SerializeField] private Vector2 _tableItemLeftOffset;
    [SerializeField] private Vector2 _tableItemRightOffset;
    [SerializeField] private Vector2 _tableOffset;

    private CustomerData _data;
    private CustomerState? _stateO;
    private Material _materialO;
    private ItemId[] _desiredItemsIds;
    private Item[] _desiredItems;
    private Item _chairItem;
    private Item _moneyItem;
    private Item _thinkBubble;
    private Item _thinkDots;
    private Item _thinkBubbleTable;

    public event EventHandler Leave;

    public MeshRenderer MeshRenderer { get; private set; }

    public CustomerData Data
    {
        get => _data;
        set => UpdateData(value);
    }

    public Table Table { get; set; }
    public CustomerState State { get; private set; }

    public override void Awake()
    {
        base.Awake();
        
        InitializeItems();
        MeshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    public void Start()
    {
        SetStatus(CustomerState.WaitingForSeat);
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
        _desiredItemsIds = Data.DesiredItems.ToArray();
    }

    public bool TryCheckout()
    {
        if (State != CustomerState.WaitingForCheckout) return false;
        Leave?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
        return true;
    }

    public bool TryReceiveMeal()
    {
        if (State != CustomerState.WaitingForMeal) return false;
        StartCoroutine(nameof(StartEating));
        return true;
    }

    public IEnumerator StartEating()
    {
        SetStatus(CustomerState.Eating);
        yield return new WaitForSeconds(GameSettings.Data.CustomerEatingTime);
        SetStatus(CustomerState.WaitingForCheckout);
    }

    public void OnSeated()
    {
        SetStatus(CustomerState.ThinkingAboutMeal);
        StartCoroutine(nameof(OnThinkingStart));
    }

    public IEnumerator OnThinkingStart()
    {
        yield return new WaitForSeconds(GameSettings.Data.CustomerThinkingTime);
        SetStatus(CustomerState.WaitingForMeal);
    }

    private void SetStatus(CustomerState state)
    {
        State = state;
        UpdateVisualization();
        _stateO = State;
    }

    private void UpdateVisualization()
    {
        HandleWaitingForSeat();
        HandleThinkingAboutMeal();
        HandleWaitingForMeal();
    }

    private void HandleWaitingForSeat()
    {
        if (State != CustomerState.WaitingForSeat) return;
        if (_stateO == CustomerState.WaitingForSeat) return;
        _thinkBubble.Show();
        _thinkBubble.Follow(this, _thinkBubbleOffset);
        _chairItem.Show();
        _chairItem.Follow(_thinkBubble, _thinkBubbleItemOffset);
    }

    private void HandleThinkingAboutMeal()
    {
        if (State != CustomerState.ThinkingAboutMeal) return;
        if (_stateO == CustomerState.ThinkingAboutMeal) return;
        _chairItem.Hide();
        _thinkBubble.Hide();
        _thinkBubbleTable.Show();
        _thinkBubbleTable.Follow(Table, _tableOffset);
        _thinkDots.Show();
        _thinkDots.Follow(_thinkBubbleTable, _thinkBubbleItemOffset);
    }

    private void HandleWaitingForMeal()
    {
        if (State != CustomerState.WaitingForMeal) return;
        if (_stateO == CustomerState.WaitingForMeal) return;
        _thinkDots.Hide();
        RenderDesiredItems();
    }

    private void RenderDesiredItems()
    {
        // ReSharper disable once ConvertIfStatementToSwitchStatement
        if (_desiredItemsIds.Length == 0) throw new NotSupportedException("The desired items for a customer are empty.");
        if (_desiredItemsIds.Length > 2) throw new NotSupportedException("At this point of development the customer cannot render more then 2 Items at once.");
        if (_desiredItemsIds.Length == 1)
        {
            var item = new Item(GameSettings.Data.Items.First(x => x.Id == _desiredItemsIds[0]), true);
            item.Follow(_thinkBubbleTable, _thinkBubbleItemOffset);
            item.ForwardTouchEventsTo(this);
            _desiredItems = new[] { item };
            return;
        }

        var itemL = new Item(GameSettings.Data.Items.First(x => x.Id == _desiredItemsIds[0]), true);
        var itemR = new Item(GameSettings.Data.Items.First(x => x.Id == _desiredItemsIds[1]), true);
        _desiredItems = new[] { itemL, itemR };
        itemL.Follow(_thinkBubbleTable, _tableItemLeftOffset);
        itemR.Follow(_thinkBubbleTable, _tableItemRightOffset);
        itemL.ForwardTouchEventsTo(this);
        itemR.ForwardTouchEventsTo(this);
    }

    public override bool IsSelectable() => State == CustomerState.WaitingForSeat;

    private void InitializeItems()
    {
        var items = new[]
        {
            _chairItem = new Item(GameSettings.Data.Items.First(x => x.Id == ItemId.ID_100_Chair)),
            _moneyItem = new Item(GameSettings.Data.Items.First(x => x.Id == ItemId.ID_101_Money)),
            _thinkBubble = new Item(GameSettings.Data.Items.First(x => x.Id == ItemId.ID_102_ThinkBubble)),
            _thinkDots = new Item(GameSettings.Data.Items.First(x => x.Id == ItemId.ID_103_ThinkDots)),
            _thinkBubbleTable = new Item(GameSettings.Data.Items.First(x => x.Id == ItemId.ID_104_ThinkBubbleTable)),
        };
        
        foreach (var item in items)
            item.ForwardTouchEventsTo(this);
    }

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