using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlobalTimeline : TimelineBase<GlobalTimeline>
{
    private Dictionary<int, CustomerData> _customers;
    private bool _closing;
    private bool _forceClose;
    private int _totalTime;
    
    public int SecondsUntilClosure { get; private set; }
    public bool Loading { get; private set; } = true;
    public bool DayComplete { get; set; }

    public override void Awake()
    {
        base.Awake();
        DayComplete = false;

        var operation = WaitUntilLevelLoaded(() =>
        {
            _totalTime = SecondsUntilClosure = LevelManager.CurrentLevel.GetSeconds();
            _customers = LevelManager.CurrentLevel.Customers
                .ToDictionary(x => x.GetSeconds(), y => y);
            
            Tick += OnTimelineTick;
            StartTicking();
        });
        
        StartCoroutine(operation);
    }

    public void Enable()
    {
        Active = true;
        DayComplete = false;
    }

    public void Disable()
    {
        Active = false;
        DayComplete = true;
    }

    public static IEnumerator WaitUntilTimelineLoaded()
    {
        yield return new WaitUntil(() => !Instance.Loading);
    }

    private static IEnumerator WaitUntilLevelLoaded(Action callback)
    {
        yield return new WaitUntil(() => !LevelManager.Instance.Loading);
        callback();
    }

    private void OnTimelineTick(object sender, EventArgs e)
    {
        SecondsUntilClosure--;
        HandleCustomerArrival();
        HandleStoreClosure();
        HandleTimerDisplay();
        Loading = false;
    }

    private void HandleTimerDisplay()
    {
        var currentTime = Mathf.Max(SecondsUntilClosure, 0);
        var percentage = Mathf.FloorToInt(100.0F / _totalTime * currentTime);
        BottomBar.Instance.OpenClosedSign.Current = percentage;
    }

    private void HandleStoreClosure()
    {
        if (SecondsUntilClosure != 0) return;
        StartCoroutine(nameof(CloseStore));
    }

    private void HandleCustomerArrival()
    {
        if (!_customers.ContainsKey(Ticks + 1)) return;
        
        var customer = _customers
            .First(x => x.Key == Ticks + 1)
            .Value;

        CustomerHandler.Instance.SummonNewCustomer(customer);
    }

    private IEnumerator CloseStore()
    {
        if (_closing) yield break;
        _closing = true;
        yield return CustomerHandler.Instance.WaitUntilCustomersLeave(() => _forceClose);
        MainMenu.Instance.CompleteDayMenu();
        DayComplete = true;
        Active = false;
        _closing = false;
    }

    public void SkipDay()
    {
        _forceClose = true;
        BottomBar.Instance.Score.Add(LevelManager.CurrentLevel.MaxScore - BottomBar.Instance.Score.Value);
        BottomBar.Instance.ProgressBar.SetValue(100.0F);
        
        if (LevelManager.CurrentLevel.Target && BottomBar.Instance.Bounties.GetBounties().All(x => !x.WasTarget))
        {
            var bounty = Model.Create<BountyData>(model =>
            {
                model.WasTarget = true;
                model.Species = LevelManager.CurrentLevel.Target;
            });
            
            BottomBar.Instance.Bounties.TryAdd(bounty);
        }
        
        StartCoroutine(CloseStore());
    }
}