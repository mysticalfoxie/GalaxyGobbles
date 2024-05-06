using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    public const int MAIN_LEVEL_INDEX = 1;
    
    public static int CurrentLevelIndex { get; private set; }
    public static LevelData CurrentLevel { get; private set; }
    
    public bool Loading { get; private set; }
    
    public override void Awake()
    {
        base.Awake();
        
        Loading = true;
        //var selectedLevel = LevelSelector.selectedLevel - 1;
        StartCoroutine(WaitUntilReferencesLoaded(() =>
        {
           // LoadLevel(selectedLevel < 0 ? 0 : selectedLevel);
            Loading = false;
        }));
    }

    private static IEnumerator WaitUntilReferencesLoaded(Action callback)
    {
        yield return new WaitUntil(() => References.Instance is not null);
        callback();
    }

    private static void LoadLevel(int index)
    {
        CurrentLevelIndex = index;
        CurrentLevel = LevelSettings.Data.Levels
            .OrderBy(x => x.Number)
            .ElementAt(index);
    }
}