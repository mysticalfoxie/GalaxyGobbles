using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class HowtoplayScreen : MonoBehaviour
{
    private MainMenu _mainMenu;
   [SerializeField] private GameObject[] _pages;
    private int _index;
    
    [Header("Buttons")] 
    [SerializeField] private Button _next;
    [SerializeField] private Button _previous;

    private void OnEnable()
    {
        _pages = this.GetChildren().ToArray();
        _mainMenu = GetComponentInParent<MainMenu>();
    }

    public void Next()
    {
        AudioManager.Instance.PlaySFX(AudioSettings.Data.UIOpenPopup);
        if (_index >= _pages.Length - 1) return;
        _pages[_index].SetActive(false);
        _pages[++_index].SetActive(true);
        _next.gameObject.SetActive(_index < _pages.Length - 1);
        _previous.gameObject.SetActive(_index > 0);
    }

    public void Previous()
    {
        AudioManager.Instance.PlaySFX(AudioSettings.Data.UIBack);
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
