using UnityEngine.SceneManagement;

public class BottomBar : SingletonMonoBehaviour<BottomBar>
{
    public Inventory Inventory { get; private set; }
    public Bounties Bounties { get; private set; }
    public OpenStatus OpenStatus { get; private set; }
    public DaytimeDisplay DaytimeDisplay { get; private set; }
    public ProgressBar ProgressBar { get; private set; }
    public Score Score { get; private set; }
    public LevelIndicator LevelIndicator { get; private set; }
    
    public override void Awake()
    {
        base.Awake();
        InheritedDDoL = true;
        Inventory = GetComponentInChildren<Inventory>();
        Bounties = GetComponentInChildren<Bounties>();
        OpenStatus = GetComponentInChildren<OpenStatus>();
        DaytimeDisplay = GetComponentInChildren<DaytimeDisplay>();
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
        OpenStatus.Reset();
        DaytimeDisplay.Reset();
        Bounties.Reset();
        Score.Reset();
    }
}