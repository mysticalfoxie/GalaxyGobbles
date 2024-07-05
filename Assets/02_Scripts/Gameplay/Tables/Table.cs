using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Table : Touchable
{
    private Chair[] _chairs;

    [SerializeField] private Table _neighbourTable;
    [SerializeField] private Orientation _orientation;
    [SerializeField] private Vector2 _thinkBubbleOffset;
    
    private bool _cleaning;
    private Item _tableThinkingBubbleItem;
    private Item _thinkingDotsItem;
    private Item _cleaningItem;
        
    public Orientation Orientation => _orientation;
    public Table NeighbourTable => _neighbourTable;
    public bool RequiresCleaning { get; private set; }

    public override void Awake()
    {
        base.Awake();
        
        CancelSelectionOnTouch = false;
        _chairs = GetComponentsInChildren<Chair>();
        CanSeat = _chairs.Length > 0;

        _tableThinkingBubbleItem = new Item(new(this, GameSettings.GetItemMatch(Identifiers.Value.ThinkBubbleTable)));
        _thinkingDotsItem = new Item(new(this, GameSettings.GetItemMatch(Identifiers.Value.Thinking)));
        _cleaningItem = new Item(new(this, GameSettings.GetItemMatch(Identifiers.Value.Cleaning)));
    }

    public bool CanSeat { get; private set; }
    public Customer Customer { get; private set; }

    public void Seat(Customer customer, Chair chair = null)
    {
        chair ??= _chairs.First();
        var position = chair.transform.position;
        var offset = customer.Data.Species.ChairOffsetHorizontal;
        if (chair.Side == Direction.Right) 
            offset = new Vector3(offset.x * -1.0F, offset.y, offset.z); 
        
        customer.transform.position = position + offset;
        Customer = customer;
        Customer.Table = this;
        Customer.Chair = chair;
    }

    public void SetDirty()
    {
        if (RequiresCleaning) return;
        RequiresCleaning = true;
        
        _tableThinkingBubbleItem.Show().Follow(this, _thinkBubbleOffset);
        _cleaningItem.Show().Follow(_tableThinkingBubbleItem);
    }

    protected override void OnTouch()
    {
        if (_cleaning) return;
        if (RequiresCleaning)
        {
            _cleaning = true;
            StartCoroutine(nameof(StartCleaning));
            return;
        }
        
        if (!Customer.IsAssigned()) return;
        Customer.InvokeTouch(this, EventArgs.Empty);
    }

    private IEnumerator StartCleaning()
    {
        _cleaningItem.Hide();
        _thinkingDotsItem.Show().Follow(_tableThinkingBubbleItem);
        yield return new WaitForSeconds(GameSettings.Data.TableCleaningTime);
        RequiresCleaning = false;
        _tableThinkingBubbleItem.Hide();
        _thinkingDotsItem.Hide();
    }

    public void ClearSeat()
    {
        Customer = null;
    }
}