using UnityEngine;


[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Loot/Weapon Data")]
public class WeaponLootData : LootItemData
{
    
    [Header("Weapon Specifics")]
    public GameObject weaponPrefab; 
    public int startingAmmo;
}