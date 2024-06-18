using System.Collections;
using UnityEngine;

public class CuttingBoard : Touchable
{
    [Header("Crafting Visualization")] 
    [SerializeField] private Vector2 _thinkingBubbleOffset;
    [SerializeField] private Vector2 _cuttingBoardUIOffset;
    [SerializeField] private Vector2 _cuttingBoardOffset;
    [SerializeField] private Vector2 _itemOffset;

    private bool _menuOpened;
    private Item _cuttingBoardUIItem;
    private Item _cuttingBoardItem;
    private Item _thinkingBubbleItem;
    private Item _emptyItem;

    public override void Awake()
    {
        base.Awake();

        InitializeItems();
    }

    protected override void OnTouch()
    {
        if (TryHandleOpenMenu()) return;
        TryHandleCloseMenu();
    }

    private bool TryHandleOpenMenu()
    {
        if (_menuOpened) return false;
        
        _thinkingBubbleItem.Show().AlignTo(this, _thinkingBubbleOffset);
        _cuttingBoardUIItem.Show().AlignTo(_thinkingBubbleItem, _cuttingBoardUIOffset);
        _emptyItem.Show().AlignTo(_cuttingBoardUIItem, _itemOffset);

        StartCoroutine(nameof(HandleIngredientSelection));
        
        return _menuOpened = true;
    }
    
    private void TryHandleCloseMenu()
    {
        if (!_menuOpened) return;

        _thinkingBubbleItem.Hide();
        _cuttingBoardUIItem.Hide();
        _emptyItem.Hide();
        
        _menuOpened = false;
    }

    private IEnumerator HandleIngredientSelection()
    {
        yield return SelectionSystem.Instance.WaitForIngredientSelection(ingredient =>
        {
            if (ingredient is null || !ingredient.CanBecomePoison)
                TryHandleCloseMenu();
            else
                OnIngredientSelected(ingredient);
        });
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.HSVToRGB(.03F, .7F, .7F);
        Gizmos.DrawCube(transform.position, transform.lossyScale);
    }

    private void OnIngredientSelected(ItemData data)
    {
        TryHandleCloseMenu();
        BottomBar.Instance.Inventory.AddPoison(data);
    }

    private void InitializeItems()
    {
        var items = new[]
        {
            _cuttingBoardUIItem = new Item(this, GameSettings.GetItemMatch(Identifiers.Value.CuttingBoardUI)),
            _emptyItem = new Item(this, GameSettings.GetItemMatch(Identifiers.Value.QuestionMark)),
            _thinkingBubbleItem = new Item(this, GameSettings.GetItemMatch(Identifiers.Value.ThinkBubbleCuttingBoard)),
            _cuttingBoardItem = new Item(this, GameSettings.GetItemMatch(Identifiers.Value.CuttingBoard), true)
        };
        
        _cuttingBoardItem.AlignTo(this, _cuttingBoardOffset);

        foreach (var item in items)
            item.ForwardTouchEventsTo(this);
    }
}