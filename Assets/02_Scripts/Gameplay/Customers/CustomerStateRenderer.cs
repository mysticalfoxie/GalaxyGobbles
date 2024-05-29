using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerStateRenderer : MonoBehaviour, IDisposable
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

    private Item[] _desiredItems = Array.Empty<Item>();
    private Item _chairItem;
    private Item _moneyItem;
    private Item _thinkBubble;
    private Item _thinkDots;
    private Item _thinkBubbleTable;
    private Item _thinkBubbleMultiHorizontalTable;
    private Item _thinkBubbleMultiVerticalTable;
    private Item _eatingItem;
    private Item[] _items;

    public CustomerStateMachine StateMachine { get; private set; }
    public Customer Customer { get; private set; }

    public void Initialize(CustomerStateMachine stateMachine)
    {
        StateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
        Customer = StateMachine.Customer ?? throw new ArgumentNullException(nameof(Customer));

        _items = new[]
        {
            _chairItem = new Item(this, GameSettings.GetItemById(Identifiers.Value.WaitForSeat)),
            _moneyItem = new Item(this, GameSettings.GetItemById(Identifiers.Value.WaitForCheckout)),
            _thinkBubble = new Item(this, GameSettings.GetItemById(Identifiers.Value.ThinkBubble)),
            _thinkDots = new Item(this, GameSettings.GetItemById(Identifiers.Value.Thinking)),
            _thinkBubbleTable = new Item(this, GameSettings.GetItemById(Identifiers.Value.ThinkBubbleTable)),
            _thinkBubbleMultiHorizontalTable = new Item(this, GameSettings.GetItemById(Identifiers.Value.ThinkBubbleTableMultiHorizontal)),
            _thinkBubbleMultiVerticalTable = new Item(this, GameSettings.GetItemById(Identifiers.Value.ThinkBubbleTableMultiVertical)),
            _eatingItem = new Item(this, GameSettings.GetItemById(Identifiers.Value.Eating)),
        };

        foreach (var item in _items)
            item.ForwardTouchEventsTo(Customer);
    }

    public void RenderWaitingForSeat()
    {
        _thinkBubble.Show();
        _thinkBubble.Follow(this, _thinkBubbleOffset);
        _chairItem.Show();
        _chairItem.Follow(_thinkBubble, _thinkBubbleItemOffset);
    }

    public void RenderThinkingAboutMeal()
    {
        _chairItem.Hide();
        _thinkBubble.Hide();
        _thinkBubbleTable.Show();
        _thinkBubbleTable.Follow(Customer.Table, Customer.Table.Orientation == Orientation.Horizontal ? _horizontalTableOffset : _verticalTableOffset);
        _thinkDots.Show();
        _thinkDots.Follow(_thinkBubbleTable, _thinkBubbleItemOffset);
    }

    public void RenderWaitingForMeal()
    {
        _thinkDots.Hide();
        RenderDesiredItems();
    }

    public void RenderWaitingForCheckout()
    {
        _eatingItem.Hide();
        _moneyItem.Show();
        _moneyItem.Follow(_thinkBubbleTable, _thinkBubbleItemOffset);
    }

    public void RenderEating()
    {
        _thinkBubbleMultiHorizontalTable.Hide();
        _thinkBubbleMultiVerticalTable.Hide();
        foreach (var item in _desiredItems) item.Hide();
        _thinkBubbleTable.Show();
        _thinkBubbleTable.Follow(Customer.Table, Customer.Table.Orientation == Orientation.Horizontal ? _horizontalTableOffset : _verticalTableOffset);
        _eatingItem.Show();
        _eatingItem.Follow(_thinkBubbleTable, _thinkBubbleItemOffset);
    }

    public void RefreshDesiredItems()
    {
        RenderDesiredItems();
    }

    private void RenderDesiredItems()
    {
        InitializeDesiredItems();
        InitializeThinkBubble();
        CreateDesiredItems();
    }

    private void InitializeDesiredItems()
    {
        foreach (var oldDesiredItem in _desiredItems) oldDesiredItem.Dispose();
        _desiredItems = Customer.DesiredItemIds.Count switch
        {
            0 => throw new NotSupportedException("The desired items for a customer are empty."),
            1 => new[] { new Item(this, GameSettings.GetItemById(Customer.DesiredItemIds[0]), true) },
            2 => new[]
            {
                new Item(this, GameSettings.GetItemById(Customer.DesiredItemIds[0]), true),
                new Item(this, GameSettings.GetItemById(Customer.DesiredItemIds[1]), true)
            },
            > 2 => throw new NotSupportedException("At this point of development the customer cannot render more then 2 Items at once."),
            _ => throw new NotSupportedException()
        };
    }

    private void InitializeThinkBubble()
    {
        var thinkingBubbles = new List<Item> { _thinkBubbleMultiVerticalTable, _thinkBubbleMultiHorizontalTable, _thinkBubbleTable };
        var thinkingBubble = GetTableThinkingBubble()
            .Show()
            .Follow(Customer.Table, GetTableThinkingBubbleOffset())
            .SendToBack();
        thinkingBubbles.Remove(thinkingBubble);
        foreach (var unused in thinkingBubbles.Where(unused => !unused.Hidden))
            unused.Hide();
    }

    private void CreateDesiredItems()
    {
        for (var i = 0; i < _desiredItems.Length; i++)
            _desiredItems[i]
                .Follow(
                    value: GetTableThinkingBubble(),
                    offset: _desiredItems.Length == 1 ? _thinkBubbleItemOffset : GetItemOffsetByItemIndex(i))
                .ForwardTouchEventsTo(Customer);
    }

    private Item GetTableThinkingBubble()
        => _desiredItems.Length switch
        {
            1 => _thinkBubbleTable,
            2 => Customer.Table.Orientation == Orientation.Horizontal
                ? _thinkBubbleMultiHorizontalTable
                : _thinkBubbleMultiVerticalTable,
            _ => throw new NotSupportedException()
        };

    private Vector2 GetTableThinkingBubbleOffset()
        => Customer.Table.Orientation == Orientation.Horizontal ? _horizontalTableOffset : _verticalTableOffset;

    private Vector2 GetItemOffsetByItemIndex(int index)
        => index switch
        {
            0 => Customer.Table.Orientation == Orientation.Horizontal ? _tableItemLeftOffset : _tableItemTopOffset,
            1 => Customer.Table.Orientation == Orientation.Horizontal ? _tableItemRightOffset : _tableItemBottomOffset,
            _ => throw new IndexOutOfRangeException(nameof(index))
        };

    public void Dispose()
    {
        foreach (var item in _items) item.Dispose();
        GC.SuppressFinalize(this);
    }
}