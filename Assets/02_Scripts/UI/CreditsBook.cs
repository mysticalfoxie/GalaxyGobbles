using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CreditsBook : MonoBehaviour
{
    private MainMenu _mainMenu;
    private GameObject[] _pages;
    private int _index;

    [Header("Buttons")] 
    [SerializeField] private Button _next;
    [SerializeField] private Button _previous;
    
    public void OnEnable()
    {
        _pages = this.GetChildren().ToArray();
        _mainMenu = GetComponentInParent<MainMenu>();
    }

    public void Next()
    {
        if (_index >= _pages.Length - 1) return;
        _pages[_index].SetActive(false);
        _pages[++_index].SetActive(true);
        _next.gameObject.SetActive(_index < _pages.Length - 1);
        _previous.gameObject.SetActive(_index > 0);
    }

    public void Previous()
    {
        if (_index <= 0) return;
        _pages[_index].SetActive(false);
        _pages[--_index].SetActive(true);
        _previous.gameObject.SetActive(_index > 0);
        _next.gameObject.SetActive(_index < _pages.Length - 1);
    }

    public void Back()
    {
        _mainMenu.HomeMenu(true);
    }
}
