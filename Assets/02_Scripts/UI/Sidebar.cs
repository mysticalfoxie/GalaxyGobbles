using UnityEngine.SceneManagement;

public class Sidebar : SingletonMonoBehaviour<Sidebar>
{
    public Inventory Inventory { get; private set; }
    public OpenStatus OpenStatus { get; private set; }
    public DaytimeDisplay DaytimeDisplay { get; private set; }
    
    public override void Awake()
    {
        base.Awake();
        InheritedDDoL = true;
        Inventory = GetComponentInChildren<Inventory>();
        OpenStatus = GetComponentInChildren<OpenStatus>();
        DaytimeDisplay = GetComponentInChildren<DaytimeDisplay>();
    }

    protected override void OnSceneChange(Scene scene)
    {
        Inventory.Reset();
        OpenStatus.Reset();
        DaytimeDisplay.Reset();
    }
}