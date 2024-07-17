using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class TimelineBase<T> : Singleton<T> where T : class
{
    public event EventHandler Tick;
    private bool _destroyed;

    protected int Ticks { get; private set; }
    protected bool Active { get; set; }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Active = scene.buildIndex == LevelManager.MAIN_LEVEL_INDEX;
    }

    protected override void OnSceneUnloaded(Scene scene)
    {
        if (scene.buildIndex == LevelManager.MAIN_LEVEL_INDEX)
            Active = false;
    }

    protected void StartTicking()
    {
        StartCoroutine(nameof(TimelineTick));
    }

    public IEnumerator TimelineTick()
    {
        while (!_destroyed && Active)
        {
            Tick?.Invoke(this, EventArgs.Empty);
            Ticks++;
            
            yield return new WaitForSeconds(1);
        }
    }

    public new void OnDestroy()
    {
        base.OnDestroy();
        _destroyed = true;
        Active = false;
    }
}