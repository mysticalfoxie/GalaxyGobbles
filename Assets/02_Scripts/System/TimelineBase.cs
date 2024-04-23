using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class TimelineBase<T> : SingletonMonoBehaviour<T>
{
    public event EventHandler Tick;
    private bool _destroyed;

    protected int Ticks { get; private set; }
    protected bool Active { get; set; }

    public override void Awake()
    {
        base.Awake();
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Active = scene.buildIndex == LevelManager.MAIN_LEVEL_INDEX;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if (scene.buildIndex == LevelManager.MAIN_LEVEL_INDEX)
            Active = false;
    }

    public void Start()
    {
        StartCoroutine(nameof(TimelineTick));
    }

    public IEnumerator TimelineTick()
    {
        yield return new WaitForSecondsRealtime(1);
        while (!_destroyed && Active)
        {
            Tick?.Invoke(this, EventArgs.Empty);
            Ticks++;
            yield return new WaitForSecondsRealtime(1);
        }
    }

    public void OnDestroy()
    {
        _destroyed = true;
        Active = false;
    }
}