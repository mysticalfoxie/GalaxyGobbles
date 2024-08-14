using System;
using UnityEngine;

// ReSharper disable InconsistentNaming

[Serializable]
public class LevelSaveData
{
    public LevelSaveData() { }

    public LevelSaveData(LevelData data)
    {
        Number = data.Number;
        Unlocked = Number == 1;
        Stars = default;
    }
    
    [SerializeField] public int Number;
    [SerializeField] public bool Unlocked;
    [SerializeField] public int Stars;
}