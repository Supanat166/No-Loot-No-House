using UnityEngine;

[CreateAssetMenu(fileName = "NewPlankData", menuName = "Loot/Plank Data")]
public class PlankLootData : LootItemData
{
    [Header("Plank Specifics")]
    public int amount; 
}