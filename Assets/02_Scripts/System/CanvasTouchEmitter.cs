using System;
using System.Linq;

public class CanvasTouchEmitter : Touchable
{
    protected override void OnTouch()
    {
        var touchable = GetComponentsInParent<Touchable>().FirstOrDefault(x => x.gameObject != gameObject);
        if (touchable) touchable.InvokeTouch(this, EventArgs.Empty);
    }
}