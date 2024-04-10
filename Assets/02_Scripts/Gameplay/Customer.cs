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
        //_meals = new List<MealData>(_data.DesiredMeals);
        _state = CustomerState.WaitingForSeat;
        base.Awake();
    }


    public override void OnClick()
    {
        if (Selected)
        {
            Deselect();
            return;
        }
        
        if (_state == CustomerState.WaitingForSeat && !Selected)
            SelectionHandler.Instance.Select(this);
    }

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