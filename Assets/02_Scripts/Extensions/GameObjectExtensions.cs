using System;
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

    public static IEnumerable<GameObject> GetChildrenRecursively(this GameObject gameObject, List<GameObject> list = null)
    {
        list ??= new List<GameObject>();

        var children = gameObject.GetChildren().ToArray();
        foreach (var child in children) 
            GetChildrenRecursively(child, list); 
        list.AddRange(children);

        return list;
    }

    public static IEnumerable<GameObject> GetChildrenRecursively(this MonoBehaviour monoBehaviour)
        => monoBehaviour.gameObject.GetChildrenRecursively();

    public static T GetRequiredComponent<T>(this GameObject gameObject)
        => gameObject.GetComponent<T>() ?? throw new NullReferenceException($"Cannot find a component of type {typeof(T).Name} on GameObject {gameObject.name}.");
    
    public static T GetRequiredComponentInChildren<T>(this GameObject gameObject)
        => gameObject.GetComponentInChildren<T>() ?? throw new NullReferenceException($"Cannot find a component of type {typeof(T).Name} in children of GameObject {gameObject.name}.");

    public static T GetRequiredComponent<T>(this MonoBehaviour @object)
        => @object.gameObject.GetRequiredComponent<T>();

    public static T GetRequiredComponentInChildren<T>(this MonoBehaviour @object)
        => @object.gameObject.GetRequiredComponentInChildren<T>();

    public static bool TryFindComponentInParents<T>(this GameObject gameObject, out T value) where T : Component
    {
        if (gameObject.transform.parent is null)
        {
            value = null;
            return false;
        }

        var component = gameObject.transform.parent.gameObject.GetComponent<T>();
        if (component)
        {
            value = component;
            return true;
        }
        
        return gameObject.transform.parent.gameObject.TryFindComponentInParents(out value);
    }
}