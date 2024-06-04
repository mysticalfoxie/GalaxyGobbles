using System;

public class ItemNotFoundException : Exception
{
    public ItemNotFoundException(ItemData data) : base($"Could not find an item named \"{data?.name ?? "null"}\".")
    {
    }
}