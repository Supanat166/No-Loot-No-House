using UnityEngine;
using System.Collections.Generic;

public class LootBox : MonoBehaviour
{
    // ลาก "พิมพ์เขียวไอเทม" (.asset) มาใส่ใน Inspector
    public List<LootItemData> possibleLoot;

    // ฟังก์ชันนี้จะถูกเรียกโดย "Character"
    public void Open(Character player)
    {
        if (possibleLoot.Count == 0) return;

        // 1. สุ่มไอเทม 1 ชิ้นจากลิสต์
        int index = Random.Range(0, possibleLoot.Count);
        LootItemData itemToGive = possibleLoot[index];

        Debug.Log("เปิดกล่อง! ได้รับ: " + itemToGive.name);

        // 2. ส่ง "พิมพ์เขียวไอเทม" ทั้งก้อนไปให้ผู้เล่น
        player.ReceiveItem(itemToGive);

        // 3. (Optional) เล่นเสียง/เอฟเฟกต์เปิดกล่อง

        // 4. ทำลายกล่องทิ้ง
        Destroy(gameObject);
    }
}