using System.Collections.Generic;

[System.Serializable]
public class InventorySaveData
{
    public List<ItemSlotData> items = new List<ItemSlotData>();
}

[System.Serializable]
public class ItemSlotData
{
    public string itemID;
    public int count;

    public ItemSlotData(string itemID, int count)
    {
        this.itemID = itemID;
        this.count = count;
    }
}