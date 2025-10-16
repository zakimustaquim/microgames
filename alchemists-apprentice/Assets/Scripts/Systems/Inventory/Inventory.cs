/// <summary>
/// Represents the player's inventory, managing items and their quantities.
/// </summary>
[System.Serializable]
public class Inventory
{
    public int capacity = 10;
    public InventorySlot[] slots;

    public bool AddItem(Item item, int amount)
    {
        // TODO
        return false;
    }

    public bool RemoveItem(Item item, int amount)
    {
        // TODO
        return false;
    }

    public bool HasItem(Item item, int amount)
    {
        foreach (var slot in slots)
        {
            if (slot.item == item && slot.quantity >= amount)
            {
                return true;
            }
        }
        return false;
    }
}
