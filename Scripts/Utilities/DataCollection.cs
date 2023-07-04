using UnityEngine;

[System.Serializable]
public class ItemDetails
{
    public int itemID;
    public string itemName;
    public ItemType itemType;
    public Sprite itemIcon;
    public Sprite itemOnWorldSprite;
    public string itemDescription;
    public int itemUseRadius;

    public bool canPickedup;
    public bool canDropped;
    public bool canCarried;
    public int itemPrice;

    [Range(0,1)]
    public float sellPercentage;
}

[System.Serializable]
public struct InventoryItem
{
    public int itemID;
    public int itemAmount;
}

[System.Serializable]
public struct AnimatorType
{
    public PartType partType;
    public PartName partName;
    public AnimatorOverrideController overrideController;
}

[System.Serializable]
public class SerializableVector3
{
    public float x, y, z;

    public SerializableVector3(Vector3 pos)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    public Vector2Int ToVector2Int()
    {
        return new Vector2Int((int)x, (int)y);
    }
}

//场景中物品的信息，ID和坐标
[System.Serializable]
public class SceneItem
{
    public int itemID;
    public SerializableVector3 position;
}

[System.Serializable]
public class TileProperty
{
    public Vector2Int tileCoordinate;
    public GridType gridType;
    public bool boolTypeValue;
}

//瓦片地图的信息,存储多个信息
[System.Serializable]
public class TileDetails
{
    //坐标
    public int gridX, gridY;
    public bool canDig;
    public bool canDropItem;
    public bool canPlaceFurniture;
    public bool isNPCObstacle;

    public int daysSinceDig = -1;
    public int daysSinceWatered = -1;
    public int seedItemID = -1;
    public int growthDays = -1; //种子已经成长天数
    public int daysSinceLastHarvest = -1;   //上次收割有多少天
}