using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Table : Touchable
{
    private Chair[] _chairs;

    [SerializeField] private Table _neighbourTable;
    [SerializeField] private GameObject _cleanBubble;
    [FormerlySerializedAs("_cleanBubbleItem")] [SerializeField] private Transform _cleanBubbleItemPosition;
    [SerializeField] private Transform _parent;
    
    private bool _cleaning;
    private Item _cleaningItem;
    private Item _thinkingDotsItem;

    public Table NeighbourTable => _neighbourTable;
    public bool RequiresCleaning { get; private set; }

    public override void Awake()
    {
        base.Awake();
        
        CancelSelectionOnTouch = false;
        _chairs = GetComponentsInChildren<Chair>();
        CanSeat = _chairs.Length > 0;

        _cleaningItem = new Item(new(this, GameSettings.GetItemMatch(Identifiers.Value.Cleaning)));
        _thinkingDotsItem = new Item(new(this, GameSettings.GetItemMatch(Identifiers.Value.Thinking)));
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
        
        _cleanBubble.SetActive(true);
        _cleaningItem
            .SetParent(_parent)
            .SetLocalPosition(_cleanBubbleItemPosition.localPosition)
            .SetRotation(new(0, 0, 0))
            .ForwardTouchEventsTo(this)
            .Show();
        
        _cleaningItem.GameObject.GetComponent<ScalingAnimator>().Play();
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
        
        if (!Customer) return;
        Customer.InvokeTouch(this, EventArgs.Empty);
    }

    private IEnumerator StartCleaning()
    {
        _cleaningItem.Hide();
        _thinkingDotsItem
            .SetParent(_parent)
            .SetLocalPosition(_cleanBubbleItemPosition.localPosition)
            .SetRotation(new(0, 0, 0))
            .ForwardTouchEventsTo(this)
            .Show();
        
        AudioManager.Instance.PlaySFX(AudioSettings.Data.LaserScanCleaning);
        yield return new WaitForSeconds(GameSettings.Data.TableCleaningTime);
        RequiresCleaning = false;
        _thinkingDotsItem.Hide();
        _cleanBubble.SetActive(false);
        _cleaning = false;
    }

    public void ClearSeat()
    {
        Customer = null;
    }
}