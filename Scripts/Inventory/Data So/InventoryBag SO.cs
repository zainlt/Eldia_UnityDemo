using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IventoryBag_SO", menuName = "Inventory/InventoryBag_SO")]
public class InventoryBag_SO : ScriptableObject
{
    public List<InventoryItem> itemList;
    public InventoryItem GetInventoryItem(int itemID)
    {
        return itemList.Find(i => i.itemID == itemID);
    }
}
