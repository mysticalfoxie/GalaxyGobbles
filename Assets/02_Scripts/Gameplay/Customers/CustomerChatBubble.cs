using System;

public class CustomerChatBubble : Touchable
{
    protected override void OnTouch()
    {
        var customer = GetComponentInParent<Customer>() ?? throw new Exception("The chat bubble has been touched but cannot find it's customer.");
        customer.InvokeTouch(this, EventArgs.Empty);
    }
}