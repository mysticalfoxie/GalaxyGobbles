using System;
using UnityEngine;

public class CustomerStateMachine : MonoBehaviour
{
    private CustomerState _state;
    private CustomerState? _stateO;

    public Customer Customer { get; private set; }
    public CustomerStateRenderer Renderer { get; private set; }

    public CustomerState State
    {
        get => _state;
        set => UpdateStatus(value);
    }

    private void UpdateStatus(CustomerState state)
    {
        _state = state;
        UpdateVisualization();
        _stateO = _state;
    }

    public void Awake()
    {
        Customer = this.GetRequiredComponent<Customer>();
        Renderer = this.GetRequiredComponent<CustomerStateRenderer>();
        Renderer.Initialize(this);
    }

    public void Start()
    {
        UpdateStatus(CustomerState.WaitingForSeat);
    }

    private void UpdateVisualization()
    {
        HandleWaitingForSeat();
        HandleThinkingAboutMeal();
        HandleWaitingForMeal();
        HandleEating();
        HandleDying();
        HandlePoisoned();
        HandleWaitingForCheckout();
        HandleLeaving();
    }

    private void HandleWaitingForSeat()
    {
        if (State != CustomerState.WaitingForSeat) return;
        if (_stateO == CustomerState.WaitingForSeat) return;
        Renderer.RenderWaitingForSeat();
    }

    private void HandleThinkingAboutMeal()
    {
        if (State != CustomerState.ThinkingAboutMeal) return;
        if (_stateO == CustomerState.ThinkingAboutMeal) return;
        Renderer.RenderThinkingAboutMeal();
    }

    private void HandleWaitingForMeal()
    {
        if (State != CustomerState.WaitingForMeal) return;
        if (_stateO == CustomerState.WaitingForMeal) return;
        Renderer.RenderWaitingForMeal();
    }

    private void HandlePoisoned()
    {
        if (State != CustomerState.Poisoned) return;
        if (_stateO == CustomerState.Poisoned) return;

        Renderer.RenderPoisoned();
    }

    private void HandleDying()
    {
        if (State != CustomerState.Dying) return;
        if (_stateO == CustomerState.Dying) return;
        Renderer.RenderDying();
    }

    private void HandleEating()
    {
        if (State != CustomerState.Eating) return;
        if (_stateO == CustomerState.Eating) return;
        Renderer.RenderEating();
    }

    private void HandleWaitingForCheckout()
    {
        if (State != CustomerState.WaitingForCheckout) return;
        if (_stateO == CustomerState.WaitingForCheckout) return;
        Renderer.RenderWaitingForCheckout();
    }

    private void HandleLeaving()
    {
        if (State != CustomerState.Leaving) return;
        if (_stateO == CustomerState.Leaving) return;
        Renderer.Dispose();
    }
}