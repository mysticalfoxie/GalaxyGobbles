using System;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public int LevelIndex { get; set; }
    public event Action<int> Clicked;

    public void OnClick()
    {
        Clicked?.Invoke(LevelIndex);
    }
}