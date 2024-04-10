using UnityEngine;

public class ItemProvider : TouchableMonoBehaviour
{
    [Header("Provider Data")]
    [SerializeField]
    private ItemData _item;
    private Inventory _inventory;
    public GameObject itemButton;
    
    void Start()
    {
        _inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
    }
    
    public override void OnClick()
    { 
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