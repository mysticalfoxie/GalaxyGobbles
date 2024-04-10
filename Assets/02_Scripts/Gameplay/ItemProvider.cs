using UnityEngine;

public class ItemProvider : TouchableMonoBehaviour
{
    [Header("Provider Data")]
    [SerializeField]
    private ItemType _item;
    private Inventory _inventory;
    public GameObject itemButton;
    
    void Start()
    {
        _inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
    }
    
    // This is not a Unity event. This needs to be replaced with the "Touched Event" :)
    public override void OnClick()
    { 
        Debug.Log("Ich wurde geklickt! UwU");
        for (int i = 0; i < _inventory.slots.Length; i++)
        {
            if (_inventory.isFull[i] == false)
            {
                _inventory.isFull[i] = true;
                Instantiate(itemButton, _inventory.slots[i].transform, false);
                Destroy(gameObject);
                break;
            }
        }
    }
}