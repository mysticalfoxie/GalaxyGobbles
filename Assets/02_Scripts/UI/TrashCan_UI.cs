using UnityEngine;

public class TrashCan_UI : MonoBehaviour
{
    public void TrashButton()
    {
        BottomBar.Instance.Inventory.Reset();
    }
}