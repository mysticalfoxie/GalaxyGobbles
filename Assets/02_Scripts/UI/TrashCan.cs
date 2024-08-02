using UnityEngine;

public class TrashCan : MonoBehaviour
{
    public void TrashButton()
    {
        AudioManager.Instance.PlaySFX(AudioSettings.Data.TrashSound);
        BottomBar.Instance.Inventory.Reset();
    }
}
