using System;
using UnityEngine;

public interface ISelectionHandler
{
    event EventHandler<object> Result;
    event EventHandler Cancel;
    void OnGameObjectTouched(GameObject @object, TouchEvent eventArgs);
    void OnSelectableTouched(Selectable selectable);
    void OnEnable();
    void OnDisable();
}