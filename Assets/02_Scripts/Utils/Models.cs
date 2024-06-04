using System;
using UnityEngine;

public static class Model
{
    public static T Create<T>(Action<T> postCreation = null) where T : ScriptableObject
    {
        var instance = ScriptableObject.CreateInstance<T>();
        postCreation?.Invoke(instance);
        return instance;
    }
}