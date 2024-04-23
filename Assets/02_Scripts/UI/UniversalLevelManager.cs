using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

public class UniversalLevelManager : MonoBehaviour
{
    public Sprite[] backgrounds;
    public Image background;
    public Text text;
    void Start()
    {
        var level = LevelSelector.selectedLevel;
        text.text = "Level " + level.ToString();
        background.sprite = backgrounds[level - 1];
    }
    public void BackToLevelSelection()
    {
        SceneManager.LoadScene("0.0_StartScene");
    }
}
