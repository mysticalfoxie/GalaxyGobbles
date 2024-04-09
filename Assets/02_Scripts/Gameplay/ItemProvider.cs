using UnityEngine;

public class ItemProvider : MonoBehaviour
{
    [SerializeField]
    private ItemType _item;
    
    // This is not a Unity event. This needs to be replaced with the "Touched Event" :)
    public void OnClick()
    {
        // Something like this :)
        // Inventory.Add(item);
    }
}