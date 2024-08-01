using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerStateRenderer : MonoBehaviour, IDisposable
{
    [Header("Selection Outline")] [SerializeField] [Range(1.0F, 1.25F)]
    private float _outlineThickness;

    [SerializeField] private Color _outlineColor;

    [Header("Thinking Bubbles")] [SerializeField] private RectTransform _canvasRectangle;
    [SerializeField] private RectTransform _canvasAssetsRectangle;
    [SerializeField] private GameObject _thinkingBubble;
    [SerializeField] private Vector2 _singleMealCanvasPosition;
    [SerializeField] private Vector2 _singleMealCanvasFlippedPositionOffset;
    [SerializeField] private Vector2 _singleMealCanvasScale;
    [SerializeField] private Vector2 _comboMealCanvasPosition;
    [SerializeField] private Vector2 _comboMealCanvasFlippedPositionOffset;
    [SerializeField] private Vector2 _comboMealCanvasScale;
    [SerializeField] private Transform _singleMealItemPosition;
    [SerializeField] private Transform _comboMealItem1Position;
    [SerializeField] private Transform _comboMealItem2Position;
    [SerializeField] [Range(0.25F, 2F)] private float _itemScale = 1.0F;

    private Item[] _desiredItems = Array.Empty<Item>();
    private Item _chairItem;
    private Item _moneyItem;
    private Item _thinkDots;
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
        BottomBar.Instance.Inventory.Update += OnInventoryUpdate;
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
        InitializeCustomerSprites(Customer.Data.Species);
    }

    public void RenderSeated()
    {
        //Customer.Chair.Direction
        SpriteRenderer.sprite = Customer.Data.Species.SittingSprite;
    }

    public void RenderWaitingForSeat()
    {
        RenderThinkingBubble();
        RenderItem(_chairItem);
    }

    public void RenderThinkingAboutMeal()
    {
        HideAllItems();
        RenderThinkingBubble();
        RenderItem(_thinkDots);
    }

    public void RenderWaitingForMeal()
    {
        HideAllItems();
        RenderThinkingBubble();
        RenderDesiredItems();
        CheckForNewItemMatches(BottomBar.Instance.Inventory.Items);
    }

    public void RenderWaitingForCheckout()
    {
        HideAllItems();
        RenderThinkingBubble();
        RenderItem(_moneyItem);
        _moneyItem.GameObject.GetComponent<ScalingAnimator>().Play();
    }

    public void RenderEating()
    {
        HideAllItems();
        RenderThinkingBubble();
        RenderItem(_eatingItem);
    }

    public void RenderDying()
    {
        HideAllItems();
        RenderThinkingBubble();
        RenderItem(_dyingItem);
    }

    private void HideAllItems()
    {
        foreach (var item in _items) item.Hide();
        foreach (var desiredItem in _desiredItems) desiredItem.Dispose();
    }

    public void RenderAngry()
    {
        HideAllItems();
        RenderItem(_angryItem);
    }

    public void RefreshDesiredItems()
    {
        RenderDesiredItems();
    }

    public void Dispose()
    {
        HideAllItems();
        GC.SuppressFinalize(this);
    }

    public void RenderPoisoned()
    {
        HideAllItems();
        RenderItem(_poisonedItem);
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
        _thinkDots.Hide();
        InitializeDesiredItems();
        RenderThinkingBubble(_desiredItems.Length > 1);
        CreateDesiredItems();
    }

    private void RenderThinkingBubble(bool multipleItems = false)
    {
        _thinkingBubble.SetActive(true);
        _canvasRectangle.sizeDelta = multipleItems ? _comboMealCanvasScale : _singleMealCanvasScale;
        _canvasRectangle.localPosition = multipleItems ? _comboMealCanvasPosition : _singleMealCanvasPosition;
        HandleThinkingBubbleOrientation(multipleItems);
        var boxCollider = _canvasRectangle.GetComponent<BoxCollider>();
        if (!boxCollider) return;
        boxCollider.size = _canvasRectangle.sizeDelta;
    }

    private void HandleThinkingBubbleOrientation(bool multipleItems)
    {
        if (Customer.Chair?.Side != Direction.Right) return;

        var offset = multipleItems ? _comboMealCanvasFlippedPositionOffset : _singleMealCanvasFlippedPositionOffset;
        _canvasRectangle.anchoredPosition = _canvasRectangle.anchoredPosition.ToVector3().MultiplyX(-1).Add(offset);
        if (_canvasAssetsRectangle.transform.localScale.x > 0)
            _canvasAssetsRectangle.transform.localScale = _canvasAssetsRectangle.transform.localScale.MultiplyX(-1);
    }

    private void InitializeDesiredItems()
    {
        foreach (var oldDesiredItem in _desiredItems) oldDesiredItem.Dispose();
        _desiredItems = Customer.DesiredItems.Count switch
        {
            0 => throw new NotSupportedException("The desired items for a customer are empty."),
            1 => new[] { new Item(new(this, GameSettings.GetItemMatch(Customer.DesiredItems[0]), true)) },
            2 => new[]
            {
                new Item(new(this, GameSettings.GetItemMatch(Customer.DesiredItems[0]), true)),
                new Item(new(this, GameSettings.GetItemMatch(Customer.DesiredItems[1]), true))
            },
            > 2 => throw new NotSupportedException("At this point of development the customer cannot render more then 2 Items at once."),
            _ => throw new NotSupportedException()
        };
    }

    private void OnInventoryUpdate(object sender, IReadOnlyCollection<Item> items)
    {
        if (!this || !gameObject || !isActiveAndEnabled) return;
        if (StateMachine.State != CustomerState.WaitingForMeal) return;
        CheckForNewItemMatches(items);
        CheckForOutdatedMatches(items);
    }

    private void CheckForOutdatedMatches(IEnumerable<Item> items)
    {
        var activeItems = _desiredItems
            .Select(x => new { Item = x, Animator = x.GameObject?.GetComponent<ScalingAnimator>() })
            .Where(x => x.Animator is not null)
            .Where(x => x.Animator.Playing);
        
        foreach (var container in activeItems)
            // ReSharper disable once PossibleMultipleEnumeration
            if (items.All(x => x.Data.name != container.Item.Data.name))
                container.Animator.Stop();
    }

    private void CheckForNewItemMatches(IEnumerable<Item> items)
    {
        foreach (var desiredItem in _desiredItems)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            if (items.All(y => y.Data.name != desiredItem.Data.name)) continue;
            var animator = desiredItem.GameObject.GetComponent<ScalingAnimator>();
            if (!animator) continue;
            animator.Play();
        }
    }

    private void RenderItem(Item item, Vector2? position = null)
    {
        _thinkingBubble.SetActive(true);
        item
            .SetParent(_thinkingBubble.transform)
            .SetLocalPosition(position ?? _singleMealItemPosition.localPosition)
            .SetRotation(new Vector3(0, 0, 0))
            .SetScale(new Vector3(_itemScale, _itemScale, 1.0F))
            .ForwardTouchEventsTo(Customer)
            .Show();
    }

    private void InitializeItems()
    {
        foreach (var item in _items = new[]
        {
            _chairItem = new Item(new(this, GameSettings.GetItemMatch(Identifiers.Value.WaitForSeat), dimension: ItemDisplayDimension.Dimension3D)),
            _moneyItem = new Item(new(this, GameSettings.GetItemMatch(Identifiers.Value.WaitForCheckout), dimension: ItemDisplayDimension.Dimension3D)),
            _thinkDots = new Item(new(this, GameSettings.GetItemMatch(Identifiers.Value.Thinking), dimension: ItemDisplayDimension.Dimension3D)),
            _eatingItem = new Item(new(this, GameSettings.GetItemMatch(Identifiers.Value.Eating), dimension: ItemDisplayDimension.Dimension3D)),
            _dyingItem = new Item(new(this, GameSettings.GetItemMatch(Identifiers.Value.Dying), dimension: ItemDisplayDimension.Dimension3D)),
            _poisonedItem = new Item(new(this, GameSettings.GetItemMatch(Identifiers.Value.Poisoned), dimension: ItemDisplayDimension.Dimension3D)),
            _angryItem = new Item(new(this, GameSettings.GetItemMatch(Identifiers.Value.Angry), dimension: ItemDisplayDimension.Dimension3D)),
        }) item.ForwardTouchEventsTo(Customer);
    }

    private void CreateDesiredItems()
    {
        for (var i = 0; i < _desiredItems.Length; i++)
            _desiredItems[i]
                .SetParent(_thinkingBubble.transform)
                .SetLocalPosition(_desiredItems.Length == 1
                    ? _singleMealItemPosition.localPosition
                    : i == 0
                        ? _comboMealItem1Position.localPosition
                        : _comboMealItem2Position.localPosition)
                .SetRotation(new Vector3(0, 0, 0))
                .SetScale(new Vector3(_itemScale, _itemScale, 1.0F))
                .ForwardTouchEventsTo(Customer)
                .Show();
    }

    public void OnPoisonHidden()
    {
        _thinkingBubble.SetActive(false);
        HideAllItems();
    }

    public void InitializeCustomerSprites(SpeciesData data)
    {
        SpriteRenderer = this.GetRequiredComponentInChildren<SpriteRenderer>();
        SpriteRenderer.sprite = data.FrontSprite;
        _outline.Disable();
        _outline.SetColor(_outlineColor);
        _outline.SetThickness(_outlineThickness);
        SpriteRenderer.transform.localScale = new Vector3(data.Scale, data.Scale, 1);

        var boxCollider = this.GetRequiredComponent<BoxCollider>();
        boxCollider.size = data.ColliderSize;
        boxCollider.center = Vector3.zero;
    }
}
