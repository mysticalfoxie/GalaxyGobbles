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
        _hearts = this.GetRequiredComponentInChildren<Hearts>();
        _heartsGameObject = _hearts.gameObject;
        _heartsGameObject.SetActive(false);
    }
    
    public void StartTicking()
    {
        if (!isActiveAndEnabled) return;
        Ticking = true;
        _heartsGameObject.SetActive(true);
        Value = 100.0F;
        StartCoroutine(nameof(OnStartTicking));
    }

    public void Add(float amount)
    {
        Value = Math.Min(Value + amount, 100.0F);         
        _hearts.SetFillPercentage(Value);
    }

    public PatienceCategory State => GetStateByValue();

    private PatienceCategory GetStateByValue()
    {
        if (Value <= GameSettings.Data.CustomerAngryThreshold)
            return PatienceCategory.Angry;
        if (Value < GameSettings.Data.CustomerLoveThreshold)
            return PatienceCategory.Happy;
        
        return PatienceCategory.Love;
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
        if (!this) return false;
        if (!isActiveAndEnabled) return false;
        return Value > 0.0F;
    }

    public void Dispose()
    {
        Destroy(_heartsGameObject);
    }
}