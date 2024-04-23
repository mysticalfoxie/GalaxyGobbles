using UnityEngine;
using UnityEngine.InputSystem;

public interface ITouchEventSystem
{
    bool IsTapping { get; }
    bool IsFingerDown { get; }
    bool IsFingerPressing { get; }
    bool IsFingerUp { get; }
    bool IsFingerMoving { get; }
    
    InputAction.CallbackContext? TapContext { get; }
    InputAction.CallbackContext? FingerDownContext { get; }
    InputAction.CallbackContext? FingerUpContext { get; }
    InputAction.CallbackContext? MovingContext { get; }
    
    GameObject GetTappedGameObject();
}