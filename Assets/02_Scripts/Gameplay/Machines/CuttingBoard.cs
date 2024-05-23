using UnityEngine;
using UnityEngine.Serialization;

public class CuttingBoard : TouchableMonoBehaviour
{
    [Header("Crafting Visualization")] 
    [SerializeField] private Vector2 _thinkingBubbleOffset;
    [SerializeField] private Vector2 _cuttingBoardOffset;
    [SerializeField] private Vector2 _itemOffset;

    private bool _menuOpened;
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
        HandleOpenMenu();
        HandleCloseMenu();
    }

    private void HandleOpenMenu()
    {
        if (_menuOpened) return;
        
        _thinkingBubbleItem.Show().AlignTo(this, _thinkingBubbleOffset);
        _cuttingBoardItem.Show().AlignTo(_thinkingBubbleItem);
        _emptyItem.Show().AlignTo(_cuttingBoardItem);
        
        _menuOpened = true;
    }
    
    private void HandleCloseMenu()
    {
        if (!_menuOpened) return;
        
        _thinkingBubbleItem.Hide();
        _cuttingBoardItem.Hide();
        _emptyItem.Hide();
        
        _menuOpened = false;
    }

    private void InitializeItems()
    {
        _cuttingBoardItem = new Item(GameSettings.GetItemById(ItemId.ID_109_CuttingBoard));
        _emptyItem = new Item(GameSettings.GetItemById(ItemId.ID_108_QuestionMark));
        _thinkingBubbleItem = new Item(GameSettings.GetItemById(ItemId.ID_104_ThinkBubble_Table));
    }
}