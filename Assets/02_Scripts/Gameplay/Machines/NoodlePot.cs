using System;
using System.Collections;
using UnityEngine;

public class NoodlePot : Touchable
{
    private Item _emptyPotItem;
    private Item _cookingItem;
    private Item _overcookedItem;
    private Item _cookedItem;
    private Item _item;
    private Item[] _items;

    [Header("Item Visualization")] [SerializeField] private RectTransform _canvas;
    [SerializeField] [Range(0.1F, 5.0F)] private float _scale = 1;

    public NoodlePotState State { get; private set; }

    public override void Awake()
    {
        base.Awake();
        InitializeItems();
        UpdateState(NoodlePotState.Empty);
    }

    protected override void OnTouch()
    {
        MainCharacter.Instance.MoveTo(transform, OnInteract);
    }

    private void OnInteract()
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

        var item = new Item(new(this, GameSettings.GetItemMatch(Identifiers.Value.Noodles)));
        if (!BottomBar.Instance.Inventory.TryAdd(item))
        {
            item.Dispose();
            return;
        }

        item.Show();
        
        AudioManager.Instance.PlaySFX(AudioSettings.Data.TakeReadyNoodles);
        UpdateState(NoodlePotState.Empty);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.HSVToRGB(.2F, .7F, .7F);
        Gizmos.DrawCube(transform.position, transform.lossyScale);
    }

    private void OnOvercookedNoodlesTouched()
    {
        if (State != NoodlePotState.Overcooked) return;
        UpdateState(NoodlePotState.Cleaning);
        StartCoroutine(nameof(OnCleaningStart));
    }

    private IEnumerator OnCookingFinished()
    {
        if (GlobalTimeline.Instance.DayComplete) yield break;
        UpdateState(NoodlePotState.Cooked);
        AudioManager.Instance.PlaySFX(AudioSettings.Data.Ready, true);
        var overcookTime = GameSettings.Data.NoodleOvercookTime;
        yield return new WaitForSeconds(overcookTime);
        if (GlobalTimeline.Instance.DayComplete) yield break;
        if (State != NoodlePotState.Cooked) yield break;
        AudioManager.Instance.PlaySFX(AudioSettings.Data.Overcooked, true);
        UpdateState(NoodlePotState.Overcooked);
    }

    private IEnumerator OnCleaningStart()
    {
        var cleaningTime = GameSettings.Data.PotCleaningTime;
        yield return new WaitForSeconds(cleaningTime);
        if (GlobalTimeline.Instance.DayComplete) yield break;
        AudioManager.Instance.PlaySFX(AudioSettings.Data.PotCleaning);
        UpdateState(NoodlePotState.Empty);
    }

    private IEnumerator OnCookingStart()
    {
        UpdateState(NoodlePotState.Cooking);
        AudioManager.Instance.PlaySFX(AudioSettings.Data.BoilingSounds, true);
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
        _items = new[]
        {
            _emptyPotItem = new Item(new(this, GameSettings.GetItemMatch(Identifiers.Value.NoodlePotEmpty), true, ItemDisplayDimension.Dimension3D)),
            _cookingItem = new Item(new(this, GameSettings.GetItemMatch(Identifiers.Value.NoodlePotCooking), dimension: ItemDisplayDimension.Dimension3D)),
            _overcookedItem = new Item(new(this, GameSettings.GetItemMatch(Identifiers.Value.NoodlePotOvercooked), dimension: ItemDisplayDimension.Dimension3D)),
            _cookedItem = new Item(new(this, GameSettings.GetItemMatch(Identifiers.Value.NoodlePotCooked), dimension: ItemDisplayDimension.Dimension3D))
        };

        foreach (var item in _items)
            item.ForwardTouchEventsTo(this)
                .SetParent(_canvas.transform)
                .SetLocalPosition(Vector2.zero)
                .SetRotation(Vector2.zero)
                .SetScale(new Vector2(_scale, _scale));
    }

    private void OnValidate()
    {
        GameSettings.GetItemMatch(Identifiers.Value.Noodles);
        GameSettings.GetItemMatch(Identifiers.Value.NoodlePotEmpty);
        GameSettings.GetItemMatch(Identifiers.Value.NoodlePotCooking);
        GameSettings.GetItemMatch(Identifiers.Value.NoodlePotOvercooked);
        GameSettings.GetItemMatch(Identifiers.Value.NoodlePotCooked);
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