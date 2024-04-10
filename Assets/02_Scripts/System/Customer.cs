using System.Collections.Generic;
using UnityEngine;

public class Customer : TouchableMonoBehaviour
{
    internal CustomerData _data;
    internal List<MealData> _meals;
    internal CustomerState _state;
    
    public override void Awake()
    {
        //_meals = new List<MealData>(_data.DesiredMeals);
        _state = CustomerState.WaitingForSeat;
        base.Awake();
    }

    public void FixedUpdate()
    {
        //if (_state == CustomerState.WaitingForSeat)
    }

    public override void OnClick()
    {
        Debug.Log("Customer has been touched! UwU");
    }
}