using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaitAreaHandler : Singleton<WaitAreaHandler>
{
    private readonly List<Customer> _outsideQueue = new();
    private WaitArea[][] _waitAreas;

    public override void Awake()
    {
        base.Awake();

        SetupWaitAreaArray();
    }

    public void AddCustomer(Customer customer)
    {
        var slot = _waitAreas.SelectMany(x => x).FirstOrDefault(x => x.Customer is null);
        if (slot is null)
        {
            _outsideQueue.Add(customer);
            customer.Visible = false;
            customer.gameObject.SetActive(false);

            Debug.Log("[Wait Area] A new customer waits outside and will arrive as soon as there's an open slot.");
            return;
        }
        
        AudioManager.Instance.PlaySFX(AudioSettings.Data.CustomerEnters);
        slot.SetCustomer(customer);
        customer.Visible = true;
    }

    public void RemoveCustomer(Customer customer)
    {
        if (customer is null) return;

        var slot = _waitAreas.SelectMany(x => x).FirstOrDefault(x => x.Customer == customer);
        if (slot is null) return;
        slot.RemoveCustomer();

        RestockSlots();
    }

    private void RestockSlots()
    {
        StartCoroutine(nameof(OnRestockCustomers));
    }

    private IEnumerator OnRestockCustomers()
    {
        yield return new WaitForSeconds(GameSettings.Data.QueueRestockDelay);
        for (var column = 0; column < _waitAreas.Length; column++)
        for (var spot = 0; spot < _waitAreas[column].Length; spot++)
        {
            if (_waitAreas[column][spot].Customer is not null) continue;
            if (TryGetCustomerFromSameColumn(column, spot)) continue;
            if (TryGetCustomerFromDiagonalColumn(spot, column)) continue;
            TryGetCustomerFromOutsideQueue(column, spot);
        }
    }

    private void TryGetCustomerFromOutsideQueue(int column, int spot)
    {
        var outsideCustomer = _outsideQueue.FirstOrDefault();
        if (outsideCustomer is not null)
        {
            _outsideQueue.Remove(outsideCustomer);
            _waitAreas[column][spot].SetCustomer(outsideCustomer);
            outsideCustomer.Visible = true;
        }
    }

    private bool TryGetCustomerFromDiagonalColumn(int spot, int column)
    {
        if (spot != 0) return false; // The very first slot of the queue should grab from the other sides - not the ones behind.
        
        var nextColumnIndex = column - 1 < 0 ? column + 1 >= _waitAreas.Length ? -1 : column + 1 : column - 1;
        if (nextColumnIndex == -1) return false;
        
        var spotFromNextColumn = _waitAreas[nextColumnIndex][1..].FirstOrDefault(x => x.Customer is not null);
        if (spotFromNextColumn is null) return true;
        _waitAreas[column][spot].SetCustomer(spotFromNextColumn.Customer);
        spotFromNextColumn.RemoveCustomer();
        
        return true;
    }

    private bool TryGetCustomerFromSameColumn(int column, int spot)
    {
        var nextSpot = _waitAreas[column][spot..].FirstOrDefault(x => x.Customer is not null);
        if (nextSpot is null) return false;
        
        _waitAreas[column][spot].SetCustomer(nextSpot.Customer);
        nextSpot.RemoveCustomer();
        
        return true;
    }

    private void SetupWaitAreaArray()
    {
        var spots = GetComponentsInChildren<WaitArea>();
        _waitAreas = new WaitArea[spots.Max(x => x.Column) + 1][];
        foreach (var spot in spots)
            if ((_waitAreas[spot.Column]?.Length ?? 0) > 0)
                _waitAreas[spot.Column] = _waitAreas[spot.Column].Append(spot).OrderBy(x => x.Order).ToArray();
            else
                _waitAreas[spot.Column] = new[] { spot };
    }
}