using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    public static T GetRequiredComponent<T>(this MonoBehaviour @object)
        => @object.gameObject.GetRequiredComponent<T>();

    public static bool IsAssigned(this UnityEngine.Object gameObject, Action whenNotAssigned = null)
    {
        if (gameObject is null) return false;
        if (gameObject.IsDestroyed()) return false;
        
        try
        {
            // ReSharper disable once UnusedVariable
            // This line is just to provoke an UnassignedReferenceException - not to actually do something
            // It tries to resolve the pointer on the gameobject but then throws an exception if it isn't assigned.
            // These are mostly errors when you stop the debugging session or when a hot-reload takes place. 
            var name = gameObject.name;
            return true;
        }
        catch (UnassignedReferenceException)
        {
            Debug.LogWarning("UnassignedReferenceException. The Object is not assigned.");
            whenNotAssigned?.Invoke();
            return false;
        }
        catch (MissingReferenceException)
        {
            Debug.LogWarning("UnassignedReferenceException. The Object is not assigned.");
            whenNotAssigned?.Invoke();
            return false;
        }
    }

    public static bool IsAssigned(this GameObject gameObject, Action actionsWhenNotAssigned)
    {
        var assigned = gameObject.IsAssigned();
        if (!assigned) actionsWhenNotAssigned();
        return assigned;
    }

    public static bool IsDestroyed(this MonoBehaviour behaviour)
    {
        if (behaviour is null) return true;
        if (!behaviour.gameObject.IsAssigned()) return true;
        return !behaviour.isActiveAndEnabled;
    }
}