using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Progressbar : MonoBehaviour
{
    public static int Progress;

    public static Progressbar Instance { get; set; }

    [SerializeField] private Slider _progressSlider;
    [SerializeField] [Range(0, 100)] private int _starOneReached;
    [SerializeField] [Range(0, 100)] private int _starTwoReached;
    [SerializeField] [Range(0, 100)] private int _starThreeReached;


    private void Awake()
    {
        if (!_progressSlider) _progressSlider = gameObject.GetComponentInChildren<Slider>();
    }

    private void Update()
    {
        if (_progressSlider.value >= _starOneReached)
        {
            Progress = 1;
            Debug.Log("You reached Star 1!");
        }
        else if (_progressSlider.value >= _starTwoReached)
        {
            Progress = 2;
            Debug.Log("You reached Star 2!");
        }
        else if (_progressSlider.value >= _starThreeReached)
        {
            Progress = 3;
            Debug.Log("You reached Star 3!");
        }
    }

    public void ProgressSlider(int sliderValue)
    {
        _progressSlider.value++;
    }

    public void OnClickFillProgressbar()
    {
        _progressSlider.value++;
        Debug.Log($"Set Progressbar to {_progressSlider.value}");
    }
}