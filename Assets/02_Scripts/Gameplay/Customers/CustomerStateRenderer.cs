using System;
using System.Collections;
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
    private Item _dyingItem;
    private Item _angryItem;
    private Item[] _items;
    private Item _poisonedItem;
    private SpriteRenderer _spriteRenderer;

    public CustomerStateMachine StateMachine { get; private set; }
    public Customer Customer { get; private set; }

    public Bounds Bounds => _spriteRenderer.bounds;

    public void Initialize(CustomerStateMachine stateMachine)
    {
        StateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
        Customer = StateMachine.Customer ?? throw new ArgumentNullException(nameof(Customer));
        InitializeItems();
    }
    
    public void OnCustomerDataSet()
    {
        if (Customer.Data is null) return;
        InitializeCustomerSprites();
    }

    public void RenderSeated()
    {
        //Customer.Chair.Direction
        _spriteRenderer.sprite = Customer.Data.Species.SittingSprite;
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
        _thinkBubbleTable.Show();
        _thinkBubbleTable.Follow(Customer.Table, Customer.Table.Orientation == Orientation.Horizontal ? _horizontalTableOffset : _verticalTableOffset);
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

    public void RenderDying()
    {
        // Interrupt everything!
        foreach (var item in _items) item.Hide();
        foreach (var desiredItem in _desiredItems) desiredItem.Dispose();

        _thinkBubble.Show().Follow(this, _thinkBubbleOffset);
        _dyingItem.Show();
        _dyingItem.Follow(_thinkBubble, _thinkBubbleItemOffset);
    }

    public void RenderAngry()
    {
        // Interrupt everything!
        foreach (var item in _items) item.Hide();
        foreach (var desiredItem in _desiredItems) desiredItem.Dispose();

        _thinkBubble.Show().Follow(this, _thinkBubbleOffset);
        _angryItem.Show();
        _angryItem.Follow(_thinkBubble, _thinkBubbleItemOffset);
    }

    public void RefreshDesiredItems()
    {
        RenderDesiredItems();
    }

    public void Dispose()
    {
        foreach (var item in _items) item.Dispose();
        foreach (var desiredItem in _desiredItems) desiredItem.Dispose();
        GC.SuppressFinalize(this);
    }

    public void RenderPoisoned()
    {
        _eatingItem.Hide();
        _thinkBubbleTable.Hide();
        _thinkBubble.Show().AlignTo(this, _thinkBubbleOffset);
        _poisonedItem.Show().AlignTo(_thinkBubble, _thinkBubbleItemOffset);
        
        StartCoroutine(nameof(StartPoisonCloudAnimation));
    }

    public void SetSeated()
    {
        _spriteRenderer.sprite = Customer.Data.Species.SittingSprite;
        _spriteRenderer.flipX = Customer.Chair.Side == Direction.Left;
    }
    
    public void OnSelected()
    {
        
    }

    public void OnDeselected()
    {
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
        _desiredItems = Customer.DesiredItems.Count switch
        {
            0 => throw new NotSupportedException("The desired items for a customer are empty."),
            1 => new[] { new Item(this, GameSettings.GetItemMatch(Customer.DesiredItems[0]), true) },
            2 => new[]
            {
                new Item(this, GameSettings.GetItemMatch(Customer.DesiredItems[0]), true),
                new Item(this, GameSettings.GetItemMatch(Customer.DesiredItems[1]), true)
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

    private void InitializeItems()
    {
        foreach (var item in _items = new[]
                 {
                     _chairItem = new Item(this, GameSettings.GetItemMatch(Identifiers.Value.WaitForSeat)),
                     _moneyItem = new Item(this, GameSettings.GetItemMatch(Identifiers.Value.WaitForCheckout)),
                     _thinkBubble = new Item(this, GameSettings.GetItemMatch(Identifiers.Value.ThinkBubble)),
                     _thinkDots = new Item(this, GameSettings.GetItemMatch(Identifiers.Value.Thinking)),
                     _thinkBubbleTable = new Item(this, GameSettings.GetItemMatch(Identifiers.Value.ThinkBubbleTable)),
                     _thinkBubbleMultiHorizontalTable = new Item(this, GameSettings.GetItemMatch(Identifiers.Value.ThinkBubbleTableMultiHorizontal)),
                     _thinkBubbleMultiVerticalTable = new Item(this, GameSettings.GetItemMatch(Identifiers.Value.ThinkBubbleTableMultiVertical)),
                     _eatingItem = new Item(this, GameSettings.GetItemMatch(Identifiers.Value.Eating)),
                     _dyingItem = new Item(this, GameSettings.GetItemMatch(Identifiers.Value.Dying)),
                     _poisonedItem = new Item(this, GameSettings.GetItemMatch(Identifiers.Value.Poisoned)),
                     _angryItem = new Item(this, GameSettings.GetItemMatch(Identifiers.Value.Angry)),
                 }) item.ForwardTouchEventsTo(Customer);
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
    
    private void InitializeCustomerSprites()
    {
        _spriteRenderer = this.GetRequiredComponent<SpriteRenderer>();
        _spriteRenderer.sprite = Customer.Data.Species.FrontSprite;

        var anchor = References.Instance.AnchorCustomer;
        var scaleY = anchor.gameObject.transform.localScale.y / anchor.Data.Species.Scale * Customer.Data.Species.Scale;
        var scaleX = transform.localScale.x / transform.localScale.y * scaleY;
        transform.localScale = new Vector3(scaleX, scaleY, 1.0F);
    }

    private IEnumerator StartPoisonCloudAnimation()
    {
        yield return new WaitForSeconds(GameSettings.Data.CustomerKillDelay);
        
        CustomerPoisonRenderer.Instance.PoisonHidden += OnPoisonHidden;
        CustomerPoisonRenderer.Instance.MovingEnded += OnMovingEnded;
        CustomerPoisonRenderer.Instance.StartPoisonAnimation(Customer);
        yield break;

        void OnMovingEnded(object sender, EventArgs e)
        {
            CustomerPoisonRenderer.Instance.MovingEnded -= OnMovingEnded;
            var target = Customer.Table.NeighbourTable.Customer;
            if (target is not null) target.Kill();
        }

        void OnPoisonHidden(object sender, EventArgs e)
        {
            CustomerPoisonRenderer.Instance.PoisonHidden -= OnPoisonHidden;
            
            _thinkBubble.Hide();
            _poisonedItem.Hide();
            
            Customer.StateMachine.State = CustomerState.WaitingForCheckout;
        }
    }
}