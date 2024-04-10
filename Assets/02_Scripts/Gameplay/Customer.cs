using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : TouchableMonoBehaviour, ISelectable
{
    [SerializeField] private Material _selectedStateMaterial;
    
    internal CustomerData _data;
    internal List<ItemData> _desiredItems;
    internal CustomerState _state;
    private MeshRenderer _renderer;
    private Material _materialO;

    public bool Selected { get; private set; }

    public override void Awake()
    {
        _renderer = gameObject.GetComponent<MeshRenderer>();
        _state = CustomerState.WaitingForSeat;
        base.Awake();
    }


    public override void OnClick()
    {
        HandleSelection();
        
    }

    public void OnSeated()
    {
        _state = CustomerState.ThinkingAboutMeal;
        StartCoroutine(nameof(OnThinkingStart));
    }

    public IEnumerator OnThinkingStart()
    {
        yield return new WaitForSeconds(3);
    }

    private void HandleSelection()
    {
        switch (Selected)
        {
            case true:
                Deselect();
                return;
            case false when IsSelectable():
                SelectionHandler.Instance.Select(this);
                break;
        }
    }

    private bool IsSelectable()
        => _state switch
        {
            CustomerState.Eating => false,
            CustomerState.ThinkingAboutMeal => false,
            CustomerState.WaitingForCheckout => false,
            CustomerState.WaitingForMeal => false,
            CustomerState.WaitingForSeat => true,
            _ => false,
        };

    // TODO: Outsource to "SelectableMonoBehaviour"
    public void Select()
    {
        _materialO = _renderer.material;
        _renderer.material = _selectedStateMaterial;
        Selected = true;
    }

    public void Deselect()
    {
        _renderer.material = _materialO;
        Selected = false;
    }
}