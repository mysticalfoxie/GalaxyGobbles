using UnityEngine.SceneManagement;

public class BottomBar : Singleton<BottomBar>
{
    public Inventory Inventory { get; private set; }
    public Bounties Bounties { get; private set; }
    public OpenClosedSign OpenClosedSign { get; private set; }
    public ProgressBar ProgressBar { get; private set; }
    public Score Score { get; private set; }
    public LevelIndicator LevelIndicator { get; private set; }
    
    public override void Awake()
    {
        base.Awake();
        InheritedDDoL = true;
        Inventory = GetComponentInChildren<Inventory>();
        Bounties = GetComponentInChildren<Bounties>();
        OpenClosedSign = GetComponentInChildren<OpenClosedSign>();
        ProgressBar = GetComponentInChildren<ProgressBar>();
        Score = GetComponentInChildren<Score>();
        LevelIndicator = GetComponentInChildren<LevelIndicator>();
    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != LevelManager.MAIN_LEVEL_INDEX) return;
        ProgressBar.OnLevelLoaded();
        LevelIndicator.OnLevelLoaded();
    }

    protected override void OnSceneUnloaded(Scene scene)
    {
        Inventory.Reset();
        OpenClosedSign.Reset();
        Bounties.Reset();
        Score.Reset();
    }
}