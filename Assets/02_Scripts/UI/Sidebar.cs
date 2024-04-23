using UnityEngine;

public class Sidebar : MonoBehaviour
{
    public static Sidebar Instance { get; private set; }
    
    public Inventory Inventory { get; private set; }
    public OpenStatus OpenStatus { get; private set; }
    public DaytimeDisplay DaytimeDisplay { get; private set; }
    
    public void Awake()
    {
        if (Instance is not null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;

        Inventory = GetComponentInChildren<Inventory>();
        OpenStatus = GetComponentInChildren<OpenStatus>();
        DaytimeDisplay = GetComponentInChildren<DaytimeDisplay>();
    }
}