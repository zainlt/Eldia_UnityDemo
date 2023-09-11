public enum ItemType
{
    Seed, Commodity, Furniture,
    HoeTool, ChopTool, BreakTool, ReapTool, WaterTool, CollectTool,
    ReapableScenery
}

public enum SlotType
{
    Bag, Box, Shop
}
public enum InventoryLocation
{
    Player, Box
}

//��������
public enum PartType
{
    None, Carry, Hoe, Break, Chop
}

//������λ��
public enum PartName
{
    Body, Hair, Tool
}

public enum Season
{
    ����, ����, ����, ����
}

public enum GridType
{
    Diggable, DropItem, PlaceFurniture, NPCObstacle
}

public enum GameState
{
    GamePlay, Pause
}

public enum LightShift
{
    Morning, Night
}

public enum SoundName
{
    none, FootStepSoft, FootStepHard,
    Axe, Pickaxe, Hoe, Reap, Water, Basket,
    Pickup, Plant, TreeFalling, Rustle,
    AmbientCountryside1, AmbientCountryside2, MusicCalm1, MusicCalm2, MusicCalm3, MusicCalm4, MusicCalm5, MusicCalm6, AmbientIndoor1
}