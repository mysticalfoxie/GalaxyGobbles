using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClearAllStars : MonoBehaviour
{
    private Button _button;

#if UNITY_EDITOR
    public void Awake()
    {
        if (!Debug.isDebugBuild)
        {
            Destroy(gameObject);
            return;
        }
        
        _button = this.GetRequiredComponent<Button>();
        _button.onClick.AddListener(OnClicked);
    }
#else
    public void Awake()
    {
        Destroy(gameObject);
    }
#endif

    public static void OnClicked()
    {
        PlayerPrefs.SetInt("Stars", 00);
        PlayerPrefs.SetInt("TotalStars", 0);
        PlayerPrefs.SetInt("UnlockedLevels", 0);
        LevelButton.UnlockedLevels = 0;
        Debug.Log("[Clear Stars] All Stars cleared and Levels reset!!");
        PlayerPrefs.Save();
    } 
}
