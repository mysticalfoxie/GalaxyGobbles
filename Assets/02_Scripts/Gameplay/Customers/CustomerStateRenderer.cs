using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerStateRenderer : MonoBehaviour, IDisposable
{
    [Header("Selection Outline")] [SerializeField] private float _outlineThickness;
    [SerializeField] private Color _outlineColor;

    [Header("Item Positioning")] [SerializeField] private Vector2 _thinkBubbleItemOffset;
    [SerializeField] private Vector2 _tableItemLeftOffset;
    [SerializeField] private Vector2 _tableItemTopOffset;
    [SerializeField] private Vector2 _tableItemRightOffset;
    [SerializeField] private Vector2 _tableItemBottomOffset;

    private Item[] _desiredItems = Array.Empty<Item>();
    private Item _chairItem;
    private Item _moneyItem;
    private Item _thinkBubble;
    private Item _thinkDots;
    private Item _thinkBubbleMeals;
    private Item _thinkBubbleMultiHorizontalTable;
    private Item _thinkBubbleMultiVerticalTable;
    private Item _eatingItem;
    private Item _dyingItem;
    private Item _angryItem;
    private Item[] _items;
    private Item _poisonedItem;
    private Outline _outline;

    public SpriteRenderer SpriteRenderer { get; private set; }
    public CustomerStateMachine StateMachine { get; private set; }
    public Customer Customer { get; private set; }

    public Bounds Bounds => SpriteRenderer.bounds;

    public void Initialize(CustomerStateMachine stateMachine)
    {
        StateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
        Customer = StateMachine.Customer ?? throw new ArgumentNullException(nameof(Customer));
        _outline = GetComponent<Outline>() ?? GetComponentInChildren<Outline>() ?? throw new MissingComponentException();
        InitializeItems();
    }

    public void InitializeInEditorMode()
    {
        SpriteRenderer = this.GetRequiredComponentInChildren<SpriteRenderer>();
        _outline = GetComponent<Outline>() ?? GetComponentInChildren<Outline>() ?? throw new MissingComponentException();
    }

    public void OnCustomerDataSet()
    {
        if (Customer.Data is null) return;
        InitializeCustomerSprites();
    }

    public void RenderSeated()
    {
        //Customer.Chair.Direction
        SpriteRenderer.sprite = Customer.Data.Species.SittingSprite;
    }

    public void RenderWaitingForSeat()
    {
        _thinkBubble.Show();
        _thinkBubble.Follow(this, Customer.Data.Species.ThinkBubbleOffset);
        _chairItem.Show();
        _chairItem.Follow(_thinkBubble, _thinkBubbleItemOffset);
    }

    public void RenderThinkingAboutMeal()
    {
        _chairItem.Hide();
        _thinkBubble.Hide();
        _thinkBubbleMeals.Show();
        _thinkBubbleMeals.Follow(Customer, GetThinkingBubbleMealsOffset());
        _thinkDots.Show();
        _thinkDots.Follow(_thinkBubbleMeals, _thinkBubbleItemOffset);
    }

    public void RenderWaitingForMeal()
    {
        _thinkDots.Hide();
        RenderDesiredItems();
    }

    public void RenderWaitingForCheckout()
    {
        _eatingItem.Hide();
        _thinkBubbleMeals.Show();
        _thinkBubbleMeals.Follow(Customer, GetThinkingBubbleMealsOffset());
        _moneyItem.Show();
        _moneyItem.Follow(_thinkBubbleMeals, _thinkBubbleItemOffset);
    }

    public void RenderEating()
    {
        _thinkBubbleMultiHorizontalTable.Hide();
        _thinkBubbleMultiVerticalTable.Hide();
        foreach (var item in _desiredItems) item.Hide();
        _thinkBubbleMeals.Show();
        _thinkBubbleMeals.Follow(Customer, GetThinkingBubbleMealsOffset());
        _eatingItem.Show();
        _eatingItem.Follow(_thinkBubbleMeals, _thinkBubbleItemOffset);
    }

    private Vector2 GetThinkingBubbleMealsOffset()
    {
        var offset = Customer.Table.Orientation == Orientation.Horizontal
            ? Customer.Data.Species.MealsThinkBubbleOffsetHorizontal
            : Customer.Data.Species.MealsThinkBubbleOffsetVertical;
        if (Customer.Chair.Side == Direction.Right) offset = new Vector2(offset.x * -1, offset.y);
        return offset;
    }

    public void RenderDying()
    {
        // Interrupt everything!
        foreach (var item in _items) item.Hide();
        foreach (var desiredItem in _desiredItems) desiredItem.Dispose();

        _thinkBubble.Show().Follow(this, Customer.Data.Species.ThinkBubbleOffset);
        _dyingItem.Show();
        _dyingItem.Follow(_thinkBubble, _thinkBubbleItemOffset);
    }

    public void RenderAngry()
    {
        // Interrupt everything!
        foreach (var item in _items) item.Hide();
        foreach (var desiredItem in _desiredItems) desiredItem.Dispose();

        _thinkBubble.Show().Follow(this, Customer.Data.Species.ThinkBubbleOffset);
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
        _thinkBubbleMeals.Hide();
        _thinkBubble.Show().Follow(this, Customer.Data.Species.ThinkBubbleOffset);
        _poisonedItem.Show().Follow(_thinkBubble, _thinkBubbleItemOffset);

        StartCoroutine(nameof(StartPoisonCloudAnimation));
    }

    public void SetSeated()
    {
        SpriteRenderer.sprite = Customer.Data.Species.SittingSprite;
        SpriteRenderer.flipX = Customer.Chair.Side == Direction.Left;
    }

    public void OnSelected() => _outline.Enable();
    public void OnDeselected() => _outline.Disable();

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
        var thinkingBubbles = new List<Item> { _thinkBubbleMultiVerticalTable, _thinkBubbleMultiHorizontalTable, _thinkBubbleMeals };
        var thinkingBubble = GetTableThinkingBubble()
            .Show()
            .Follow(Customer, GetThinkingBubbleMealsOffset())
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
                     _thinkBubbleMeals = new Item(this, GameSettings.GetItemMatch(Identifiers.Value.ThinkBubbleTable)),
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
            1 => _thinkBubbleMeals,
            2 => Customer.Table.Orientation == Orientation.Horizontal
                ? _thinkBubbleMultiHorizontalTable
                : _thinkBubbleMultiVerticalTable,
            _ => throw new NotSupportedException()
        };

    private Vector2 GetItemOffsetByItemIndex(int index)
        => index switch
        {
            0 => Customer.Table.Orientation == Orientation.Horizontal ? _tableItemLeftOffset : _tableItemTopOffset,
            1 => Customer.Table.Orientation == Orientation.Horizontal ? _tableItemRightOffset : _tableItemBottomOffset,
            _ => throw new IndexOutOfRangeException(nameof(index))
        };

    private void InitializeCustomerSprites()
    {
        var anchor = References.Instance.AnchorCustomer;
        InitializeCustomerSprites(Customer.Data.Species, anchor, anchor.Data.Species);
    }

    public void InitializeCustomerSprites(SpeciesData data, Customer anchor, SpeciesData anchorSpecies)
    {
        SpriteRenderer = this.GetRequiredComponentInChildren<SpriteRenderer>();
        SpriteRenderer.sprite = data.FrontSprite;
        _outline.Disable();
        _outline.SetColor(_outlineColor);
        _outline.SetThickness(_outlineThickness);
        var scaleY = anchor.gameObject.transform.localScale.y / anchorSpecies.Scale * data.Scale;
        var scaleX = transform.localScale.x / transform.localScale.y * scaleY;
        transform.localScale = new Vector3(scaleX, scaleY, 1.0F);
        var boxCollider = this.GetRequiredComponent<BoxCollider>();
        boxCollider.size = data.ColliderSize;
        boxCollider.center = Vector3.zero;
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