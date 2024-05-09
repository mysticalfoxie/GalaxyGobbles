using System;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CustomerStateMachine : MonoBehaviour
{
    [Header("Item Positioning")]
    [SerializeField] private Vector2 _thinkBubbleOffset;
    [SerializeField] private Vector2 _thinkBubbleItemOffset;
    [SerializeField] private Vector2 _tableItemLeftOffset;
    [SerializeField] private Vector2 _tableItemTopOffset;
    [SerializeField] private Vector2 _tableItemRightOffset;
    [SerializeField] private Vector2 _tableItemBottomOffset;
    [SerializeField] private Vector2 _horizontalTableOffset;
    [SerializeField] private Vector2 _verticalTableOffset;
    
    private CustomerState _state;
    private CustomerState? _stateO;
    private ItemId[] _desiredItemsIds;
    private Item[] _desiredItems;
    private Item _chairItem;
    private Item _moneyItem;
    private Item _thinkBubble;
    private Item _thinkDots;
    private Item _thinkBubbleTable;
    private Item _thinkBubbleMultiHorizontalTable;
    private Item _thinkBubbleMultiVerticalTable;
    private Item _eatingItem;
    private Item[] _items;

    public Customer Customer { get; private set; }
    
    public CustomerState State
    {
        get => _state;
        set => UpdateStatus(value);
    }
    
    private void UpdateStatus(CustomerState state)
    {
        State = state;
        UpdateVisualization();
        _stateO = State;
    }

    public void Awake()
    {
        InitializeItems();
        Customer = this.GetRequiredComponent<Customer>();
        Customer.DataChange += (_, _) => OnCustomerDataChange();
    }

    private void OnCustomerDataChange()
    {
        _desiredItemsIds = Customer.Data.DesiredItems.ToArray();
    }

    public void Start()
    {
        UpdateStatus(CustomerState.WaitingForSeat);
    }

    private void UpdateVisualization()
    {
        HandleWaitingForSeat();
        HandleThinkingAboutMeal();
        HandleWaitingForMeal();
        HandleEating();
        HandleWaitingForCheckout();
        HandleLeaving();
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
        _thinkBubbleTable.Follow(Customer.Table, Customer.Table.Orientation == Orientation.Horizontal ? _horizontalTableOffset : _verticalTableOffset);
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

    private void HandleEating()
    {
        if (State != CustomerState.Eating) return;
        if (_stateO == CustomerState.Eating) return;
        _thinkBubbleMultiHorizontalTable.Hide();
        _thinkBubbleMultiVerticalTable.Hide();
        foreach (var item in _desiredItems) item.Hide();
        _thinkBubbleTable.Show();
        _thinkBubbleTable.Follow(Customer.Table, Customer.Table.Orientation == Orientation.Horizontal ? _horizontalTableOffset : _verticalTableOffset);
        _eatingItem.Show();
        _eatingItem.Follow(_thinkBubbleTable, _thinkBubbleItemOffset);
    }

    private void HandleWaitingForCheckout()
    {
        if (State != CustomerState.WaitingForCheckout) return;
        if (_stateO == CustomerState.WaitingForCheckout) return;
        _eatingItem.Hide();
        _moneyItem.Show();
        _moneyItem.Follow(_thinkBubbleTable, _thinkBubbleItemOffset);
    }

    private void HandleLeaving()
    {
        if (State != CustomerState.Leaving) return;
        if (_stateO == CustomerState.Leaving) return;
        foreach (var item in _items) item.Dispose();
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
            item.ForwardTouchEventsTo(Customer);
            _desiredItems = new[] { item };
            return;
        }
        
        _thinkBubbleTable.Hide();
        var tableThinkBubbleItem = Customer.Table.Orientation == Orientation.Horizontal ? _thinkBubbleMultiHorizontalTable : _thinkBubbleMultiVerticalTable;
        tableThinkBubbleItem.Show();
        tableThinkBubbleItem.Follow(Customer.Table, Customer.Table.Orientation == Orientation.Horizontal ? _horizontalTableOffset : _verticalTableOffset);
        
        var itemL = new Item(GameSettings.Data.Items.First(x => x.Id == _desiredItemsIds[0]), true);
        var itemR = new Item(GameSettings.Data.Items.First(x => x.Id == _desiredItemsIds[1]), true);
        _desiredItems = new[] { itemL, itemR };
        itemL.Follow(tableThinkBubbleItem, Customer.Table.Orientation == Orientation.Horizontal ? _tableItemLeftOffset : _tableItemTopOffset);
        itemL.ForwardTouchEventsTo(Customer);
        itemR.Follow(tableThinkBubbleItem, Customer.Table.Orientation == Orientation.Horizontal ? _tableItemRightOffset : _tableItemBottomOffset);
        itemR.ForwardTouchEventsTo(Customer);
    }
    
    private void InitializeItems()
    {
        _items = new[]
        {
            _chairItem = new Item(GameSettings.Data.Items.First(x => x.Id == ItemId.ID_100_Chair)),
            _moneyItem = new Item(GameSettings.Data.Items.First(x => x.Id == ItemId.ID_101_Money)),
            _thinkBubble = new Item(GameSettings.Data.Items.First(x => x.Id == ItemId.ID_102_ThinkBubble)),
            _thinkDots = new Item(GameSettings.Data.Items.First(x => x.Id == ItemId.ID_103_ThinkDots)),
            _thinkBubbleTable = new Item(GameSettings.Data.Items.First(x => x.Id == ItemId.ID_104_ThinkBubble_Table)),
            _thinkBubbleMultiHorizontalTable = new Item(GameSettings.Data.Items.First(x => x.Id == ItemId.ID_105_ThinkBubble_Table_Multi_Horizontal)),
            _thinkBubbleMultiVerticalTable = new Item(GameSettings.Data.Items.First(x => x.Id == ItemId.ID_106_ThinkBubble_Table_Multi_Vertical)),
            _eatingItem = new Item(GameSettings.Data.Items.First(x => x.Id == ItemId.ID_107_Eating)),
        };
        
        foreach (var item in _items)
            item.ForwardTouchEventsTo(Customer);
    }
}