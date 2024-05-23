using UnityEngine;

public interface ISelectionHandler
{
    void OnGameObjectTouched(GameObject @object);
    void OnSelectableTouched(SelectableMonoBehaviour selectable);
    void OnEnable();
    void OnDisable();
}