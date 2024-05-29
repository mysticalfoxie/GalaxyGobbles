using System;

public class ItemNotFoundException : Exception
{
    public ItemNotFoundException(string itemId) : base($"Could not find an item with id \"{itemId}\".")
    {
    }
}