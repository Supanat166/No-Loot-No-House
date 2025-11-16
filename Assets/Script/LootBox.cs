using UnityEngine;
using System.Collections.Generic;

public class LootBox : MonoBehaviour
{
    // [เพิ่ม 1] สร้างช่องสำหรับใส่ไฟล์เสียง
    [Header("Audio")] public AudioClip openSound;

    [Header("Loot")] public List<LootItemData> possibleLoot;

    // ฟังก์ชันนี้จะถูกเรียกโดย "Character"
    public void Open(Character player)
    {
        if (possibleLoot.Count == 0) return;

        // --- [เพิ่ม 2] เล่นเสียงเปิดกล่อง ---
        // (เราเล่นเสียง "ณ ตำแหน่งนี้" ก่อนที่กล่องจะหายไป)
        if (openSound != null)
        {
            AudioSource.PlayClipAtPoint(openSound, transform.position);
        }
        // ---------------------------------

        // 1. สุ่มไอเทม 1 ชิ้นจากลิสต์
        int index = Random.Range(0, possibleLoot.Count);
        LootItemData itemToGive = possibleLoot[index];

        Debug.Log("เปิดกล่อง! ได้รับ: " + itemToGive.name);

        // 2. ส่ง "พิมพ์เขียวไอเทม" ทั้งก้อนไปให้ผู้เล่น
        player.ReceiveItem(itemToGive);

        // 3. ทำลายกล่องทิ้ง (หลังจากเล่นเสียงและให้ของแล้ว)
        Destroy(gameObject);
    }
}