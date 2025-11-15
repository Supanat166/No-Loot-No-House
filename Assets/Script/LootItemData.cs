using UnityEngine;

public enum ItemType 
{ 
    Weapon, 
    SpeedPotion, 
    WoodPlank 
}

public class LootItemData : ScriptableObject
{
    public ItemType type;
}