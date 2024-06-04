using System;
using System.Linq;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;

public static class ScriptableObjectExtensions
{
    public static T Clone<T>(this T model) where T : ScriptableObject 
        => model.Clone(ScriptableObject.CreateInstance<T>, t =>
        {
            // Special Case. We don't want to copy all of unity's fields, just the name.
            t.name = model.name;
            t.hideFlags = model.hideFlags;
            return t;
        });

    public static T MemberwiseClone<T>(this T model) where T : class 
        => model.Clone(Activator.CreateInstance<T>);

    public static T Clone<T>(this T model, Func<T> instantiator, Func<T, T> postCloneActions = null) where T : class
    {
        var clone = instantiator();
        var properties = typeof(T)
            .GetDeclaredProperties()
            .Where(x => x.CanRead && x.CanWrite); // Only Properties with { get; set; }

        var fields = typeof(T).GetDeclaredFields()
            .Where(x => !x.IsStatic) // Only entries on the model
            .Where(x => !x.Name.Contains('<'))
            .ToList(); // Excluding Property BackingFields which are called:  <Type>k__BackingField
        
        foreach (var property in properties)
            property.SetValue(clone, property.GetValue(model));

        foreach (var field in fields)
            field.SetValue(clone, field.GetValue(model));

        postCloneActions?.Invoke(clone);
        
        return clone;
    }
}