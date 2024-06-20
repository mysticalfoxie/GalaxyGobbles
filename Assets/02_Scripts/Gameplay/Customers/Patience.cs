using System;
using System.Collections;
using UnityEngine;

public class Patience : MonoBehaviour
{
    private GameObject _heartsGameObject;
    private Hearts _hearts;
    private bool _hasTicked;

    public static bool Disabled { get; set; }
    public Customer Customer { get; set; }
    public float Value { get; private set; }
    public bool Ticking { get; private set; }

    public event EventHandler Angry; 

    public void Awake()
    {
        var overlay = Overlay.Instance.gameObject.transform;
        _heartsGameObject = Instantiate(GameSettings.Data.PRE_Hearts);
        _heartsGameObject!.transform.SetParent(overlay);
        _hearts = _heartsGameObject.GetRequiredComponent<Hearts>();
        _heartsGameObject.SetActive(false);
    }
    
    public void StartTicking()
    {
        if (!isActiveAndEnabled) return;
        Ticking = true;
        _heartsGameObject.SetActive(true);
        _hearts.Follow(Customer, Customer.Data.Species.HeartsOffset);
        Value = 100.0F;
        StartCoroutine(nameof(OnStartTicking));
    }

    public void UpdateOffset()
    {
        _hearts.Offset = GetOffset();
    }

    private Vector2 GetOffset()
    {
        if (Customer.Chair is null) return Customer.Data.Species.HeartsOffset;
        if (Customer.Chair.Side != Direction.Right) return Customer.Data.Species.HeartsOffset;
        return Customer.Data.Species.HeartsOffset * new Vector2(-1, 1);
    }

    private IEnumerator OnStartTicking()
    {
        while (CanTick())
        {
            yield return new CancellableWaitForSeconds(GameSettings.Data.PatienceTickDelay, () => !CanTick());
            if (CanTick()) OnTick();
        }

        if (Value <= 0.0F) 
            Angry?.Invoke(this, EventArgs.Empty);
    }

    private void OnTick()
    {
        if (Disabled) return;
        _hasTicked = true;
        Value -= GameSettings.Data.PatienceDropAmount;
        _hearts.SetFillPercentage(Value);
    }

    private bool CanTick()
    {
        if (!_hasTicked) return true;
        if (!this.IsAssigned()) return false;
        if (!isActiveAndEnabled) return false;
        return Value > 0.0F;
    }

    public void Dispose()
    {
        Destroy(_heartsGameObject);
    }
}