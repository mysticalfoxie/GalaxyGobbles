using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    public static IEnumerable<GameObject> GetChildren(this GameObject gameObject)
    {
        for (var i = 0; i < gameObject.transform.childCount; i++)
            yield return gameObject.transform.GetChild(i).gameObject;
    }
    
    public static IEnumerable<GameObject> GetChildren(this MonoBehaviour monoBehaviour)
    {
        return monoBehaviour.gameObject.GetChildren();
    }
}