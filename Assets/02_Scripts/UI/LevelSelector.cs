using TMPro;
using UnityEngine;
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
    
    public void OpenScene()
    {
        selectedLevel = _level;
        //LevelManager();
        MainMenu.Instance.SetElementsForStart();
        SceneManager.LoadScene("0.2_Level");
    }
}
