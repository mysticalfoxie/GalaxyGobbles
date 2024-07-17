using System;
using UnityEngine;

public class TouchFeedback : Singleton<TouchFeedback>
{
    public TouchFeedback() : base(true) { }

    public GameObject _touchAnimation;

    public void TryPlayFeedbackAnimation(Vector2 position)
    {
        try
        {
            PlayFeedbackAnimation(position);
        }
        catch (Exception ex)
        {
            Debug.LogError(new Exception("An error occurred when playing the touch feedback animation.", ex));
        }
    }

    private void PlayFeedbackAnimation(Vector2 position)
    {
        var touchAnimation = Instantiate(_touchAnimation);
        touchAnimation.GetRequiredComponent<>()
        touchAnimation.transform!.SetParent(UI.Instance.transform, true);
    }
}