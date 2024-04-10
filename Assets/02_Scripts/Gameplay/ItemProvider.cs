using UnityEngine;

public class ItemProvider : TouchableMonoBehaviour
{
    [Header("Provider Data")]
    [SerializeField]
    private ItemType _item;
    
    // This is not a Unity event. This needs to be replaced with the "Touched Event" :)
    public override void OnClick()
    {
        // Something like: Inventory.Instance.Add(item)
    }
}