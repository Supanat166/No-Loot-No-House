using UnityEngine;

[CreateAssetMenu(fileName = "NewPotionData", menuName = "Loot/Potion Data")]
public class PotionLootData : LootItemData
{
    [Header("Potion Specifics")]
    public float boostAmount; 
    public float duration;    
}