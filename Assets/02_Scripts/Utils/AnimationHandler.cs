using System;

public class AnimationHandler : Singleton<AnimationHandler>
{
    public event EventHandler Tick;

    public void Update()
    {
        Tick?.Invoke(this, EventArgs.Empty);
    }
}