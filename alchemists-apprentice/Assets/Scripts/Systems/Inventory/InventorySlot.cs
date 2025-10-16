/// <summary>
/// Represents a slot in the inventory, holding an item and its quantity.
/// </summary>
[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int quantity;

    public InventorySlot(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public bool IsFull => item != null && quantity >= item.maxStack;

    public bool IsEmpty => item == null || quantity <= 0;

    public void Clear()
    {
        item = null;
        quantity = 0;
    }
}
