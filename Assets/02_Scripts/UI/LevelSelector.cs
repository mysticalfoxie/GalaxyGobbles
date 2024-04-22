using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private int _level;
    public static int selectedLevel;
    [SerializeField] TMP_Text _levelText;
    void Start()
    {
        _levelText.text = _level.ToString();
    }
    
    void OpenScene()
    {
        selectedLevel = _level;
        SceneManager.LoadScene("UniversalLevel");
    }
}
