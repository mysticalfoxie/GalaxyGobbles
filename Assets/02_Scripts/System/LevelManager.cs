using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    public const int MAIN_LEVEL_INDEX = 1;

    [Header("Debug Settings")]
    [Tooltip("The level you want to test, when starting debugging from the main level.")]
    [Range(0, 20)]
    [SerializeField] private int _debugLevel;

    public LevelManager() : base(true) { }
    
    public static int CurrentLevelIndex { get; private set; }
    public static LevelData CurrentLevel { get; private set; }
    
    public bool Loading { get; private set; }

    public override void Awake()
    {
        base.Awake();
        InitializeLevelForDebug();
    }

    private void InitializeLevelForDebug()
    {
        // For debugging sessions starting from the Main Level
        if (!enabled) return;
        if (SceneManager.GetActiveScene().buildIndex != MAIN_LEVEL_INDEX) return;

        var debugLevel = Math.Max(Math.Min(_debugLevel, GameSettings.Data.Levels.Count()), 0);
        StartCoroutine(LoadLevelAsync(debugLevel, true));
    }

    public IEnumerator LoadLevelAsync(int index, bool skipSceneLoad = false)
    {
        CurrentLevelIndex = index;
        CurrentLevel = GameSettings.Data.Levels
            .OrderBy(x => x.Number)
            .ElementAt(index);
         
        MainMenu.Instance.SetElementsForStart();
        if (skipSceneLoad) yield break;
        
        yield return LoadSceneAsync();
    }

    public IEnumerator LoadSceneAsync()
    {
        Loading = true;
        yield return SceneManager.LoadSceneAsync(MAIN_LEVEL_INDEX);
        Loading = false;
    }
}