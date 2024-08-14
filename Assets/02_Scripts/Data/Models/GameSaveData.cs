using System;
using UnityEngine;

// ReSharper disable InconsistentNaming

[Serializable]
public class GameSaveData
{
    [SerializeField] public LevelSaveData[] Levels = Array.Empty<LevelSaveData>();
}