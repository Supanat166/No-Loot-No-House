using UnityEngine;

public enum ItemType 
{ 
    Weapon, 
    SpeedPotion, 
    WoodPlank 
}
[CreateAssetMenu(menuName = "Loot/Loot Item Data")]
public class LootItemData : ScriptableObject
{
    public ItemType type;
    public string displayName;   // ใช้ตัวนี้ใน UI พึ่งแอดไป
    public Sprite icon; // พี่งแอดไป
}
