using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public int count;

    public InventorySlot(ItemData item, int count)
    {
        this.item = item;
        this.count = count;
    }

    public bool IsEmpty => item == null || count <= 0;

    public bool CanStack(ItemData otherItem)
    {
        if (item == null) return true;
        if (item != otherItem) return false;
        if (!item.isStackable) return false;
        return count < item.maxStack;
    }

    public int AddItem(ItemData newItem, int amount)
    {
        if (item == null)
        {
            item = newItem;
            count = Mathf.Min(amount, newItem.maxStack);
            return amount - count;
        }

        if (item == newItem && item.isStackable)
        {
            int space = item.maxStack - count;
            int addAmount = Mathf.Min(amount, space);
            count += addAmount;
            return amount - addAmount;
        }

        return amount;
    }

    public void Clear()
    {
        item = null;
        count = 0;
    }
}