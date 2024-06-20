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

        _tableThinkingBubbleItem = new Item(this, GameSettings.GetItemMatch(Identifiers.Value.ThinkBubbleTable));
        _thinkingDotsItem = new Item(this, GameSettings.GetItemMatch(Identifiers.Value.Thinking));
        _cleaningItem = new Item(this, GameSettings.GetItemMatch(Identifiers.Value.Cleaning));
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
    /*
     * Before
     * UnityEditor.TransformWorldPlacementJSON:{"position":{"x":1252.0400390625,"y":18.527524948120118,"z":-101.66825103759766},"rotation":{"x":0.2588191032409668,"y":0.0,"z":0.0,"w":0.9659258127212524},"scale":{"x":0.17376121878623963,"y":0.16284774243831635,"z":1.0}}
     *
     * After
     * UnityEditor.TransformWorldPlacementJSON:{"position":{"x":1263.800048828125,"y":47.400001525878909,"z":-110.19999694824219},"rotation":{"x":0.2588191032409668,"y":0.0,"z":0.0,"w":0.9659258127212524},"scale":{"x":0.17376121878623963,"y":0.16284774243831635,"z":1.0}}
     *
     * 
     */

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