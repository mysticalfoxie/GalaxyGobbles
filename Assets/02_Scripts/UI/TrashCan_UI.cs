
    using UnityEngine;
    using Unity.UI;

    public class TrashCan_UI : MonoBehaviour
    {

        public void TrashButton()
        {
            BottomBar.Instance.Inventory.Reset();
        }
    }