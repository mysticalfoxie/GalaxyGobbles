using UnityEngine;

public class TouchEvent
{
    public TouchEvent(GameObject touched)
    {
        Target = touched;
    }
    
    public void CancelPropagation()
    {
        Cancelled = true;
    }
    
    public GameObject Target { get; set; }
    public bool Cancelled { get; private set; }
}