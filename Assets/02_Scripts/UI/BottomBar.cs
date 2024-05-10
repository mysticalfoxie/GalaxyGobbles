using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class BottomBar : SingletonMonoBehaviour<BottomBar>
{
    public Inventory Inventory { get; private set; }
    public OpenStatus OpenStatus { get; private set; }
    public DaytimeDisplay DaytimeDisplay { get; private set; }
    public Progressbar Progressbar { get; private set; }
    
    public override void Awake()
    {
        base.Awake();
        InheritedDDoL = true;
        Inventory = GetComponentInChildren<Inventory>();
        OpenStatus = GetComponentInChildren<OpenStatus>();
        DaytimeDisplay = GetComponentInChildren<DaytimeDisplay>();
        Progressbar = this.GetRequiredComponent<Progressbar>();
    }

    protected override void OnSceneUnloaded(Scene scene)
    {
        Inventory.Reset();
        OpenStatus.Reset();
        DaytimeDisplay.Reset();
        Progressbar.Reset();
    }
}