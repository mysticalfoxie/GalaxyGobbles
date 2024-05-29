using System;
using System.Collections;
using UnityEngine;

public class NoodlePot : TouchableMonoBehaviour
{
    private Item _emptyPotItem;
    private Item _cookingItem;
    private Item _overcookedItem;
    private Item _cookedItem;
    private Item _item;

    [Header("Item Visualization")] [SerializeField] private Vector2 _itemOffset;
    
    public NoodlePotState State { get; private set; }

    public override void Awake()
    { 
        base.Awake();
        InitializeItems();
        UpdateState(NoodlePotState.Empty);
    }

    protected override void OnTouch()
    {
        if (State == NoodlePotState.Cooked) OnCookedNoodlesTouched();
        if (State == NoodlePotState.Overcooked) OnOvercookedNoodlesTouched();
    }

    public void StartCooking()
    {
        if (State != NoodlePotState.Empty) return;
        StartCoroutine(nameof(OnCookingStart));
    }

    private void OnCookedNoodlesTouched()
    {
        if (BottomBar.Instance.Inventory.IsFull()) return;
        
        var item = new Item(this, GameSettings.GetItemById(Identifiers.Value.Noodles));
        if (!BottomBar.Instance.Inventory.TryAdd(item))
        {
            item.Dispose();
            return;
        }
        
        item.Show();
        UpdateState(NoodlePotState.Empty);
    }

    private void OnOvercookedNoodlesTouched()
    {
        if (State != NoodlePotState.Overcooked) return;
        UpdateState(NoodlePotState.Cleaning);
        StartCoroutine(nameof(OnCleaningStart));
    }

    private IEnumerator OnCookingFinished()
    {
        UpdateState(NoodlePotState.Cooked);
        var overcookTime = GameSettings.Data.NoodleOvercookTime;
        yield return new WaitForSeconds(overcookTime);
        if (State != NoodlePotState.Cooked) yield break;
        UpdateState(NoodlePotState.Overcooked);
    }

    private IEnumerator OnCleaningStart()
    {
        var cleaningTime = GameSettings.Data.PotCleaningTime;
        yield return new WaitForSeconds(cleaningTime);
        UpdateState(NoodlePotState.Empty); 
    }

    private IEnumerator OnCookingStart()
    {
        UpdateState(NoodlePotState.Cooking);
        var boilingTime = GameSettings.Data.NoodleBoilingTime;
        yield return new WaitForSeconds(boilingTime);
        yield return OnCookingFinished();
    }

    private void UpdateState(NoodlePotState state)
    {
        _item?.Hide();
        _item = GetItemByState(State = state);
        _item.Show();
    }

    private void InitializeItems()
    {
        _emptyPotItem = new Item(this, GameSettings.GetItemById(Identifiers.Value.NoodlePotEmpty));
        _cookingItem = new Item(this, GameSettings.GetItemById(Identifiers.Value.NoodlePotCooking));
        _overcookedItem = new Item(this, GameSettings.GetItemById(Identifiers.Value.NoodlePotOvercooked));
        _cookedItem = new Item(this, GameSettings.GetItemById(Identifiers.Value.NoodlePotCooked));
        
        _cookingItem.AlignTo(this, _itemOffset);
        _overcookedItem.AlignTo(this, _itemOffset);
        _cookedItem.AlignTo(this, _itemOffset);
        _emptyPotItem.AlignTo(this, _itemOffset);
        
        _cookingItem.ForwardTouchEventsTo(this);
        _overcookedItem.ForwardTouchEventsTo(this);
        _cookedItem.ForwardTouchEventsTo(this);
        _emptyPotItem.ForwardTouchEventsTo(this);
    }

    private void OnValidate()
    {
        GameSettings.GetItemById(Identifiers.Value.Noodles);
        GameSettings.GetItemById(Identifiers.Value.NoodlePotEmpty);
        GameSettings.GetItemById(Identifiers.Value.NoodlePotCooking);
        GameSettings.GetItemById(Identifiers.Value.NoodlePotOvercooked);
        GameSettings.GetItemById(Identifiers.Value.NoodlePotCooked);
    }

    private Item GetItemByState(NoodlePotState state)
        => state switch
        {
            NoodlePotState.Empty => _emptyPotItem,
            NoodlePotState.Cooking => _cookingItem,
            NoodlePotState.Cooked => _cookedItem,
            NoodlePotState.Overcooked => _overcookedItem,
            NoodlePotState.Cleaning => _overcookedItem,
            _ => throw new NotSupportedException(nameof(state))
        };
}