using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelObjects : MonoBehaviour
{
    [SerializeField] public Button LevelButton;
    [SerializeField] public Image[] stars;
    [SerializeField] public TMP_Text _levelText;
    [SerializeField] public int _level;
    private LevelSelector _LevelSelector;
}
