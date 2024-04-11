using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameObjectExtensions
{
    public static IEnumerable<GameObject> GetChildren(this GameObject gameObject)
    {
        for (var i = 0; i < gameObject.transform.childCount; i++)
            yield return gameObject.transform.GetChild(i).gameObject;
    }

    public static IEnumerable<GameObject> GetChildren(this MonoBehaviour monoBehaviour)
        => monoBehaviour.gameObject.GetChildren();

    public static IEnumerable<GameObject> GetAllChildren(this GameObject gameObject)
        => gameObject.GetChildren().SelectMany(x => x.GetChildren());

    public static IEnumerable<GameObject> GetAllChildren(this MonoBehaviour monoBehaviour)
        => monoBehaviour.gameObject.GetAllChildren();
}