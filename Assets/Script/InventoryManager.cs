using UnityEngine;
using TMPro; // สำคัญ: ต้องใช้ namespace นี้สำหรับ TextMeshPro
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    // Singleton Pattern 
    public static InventoryManager Instance;

    // --- เปลี่ยนชนิดตัวแปรเป็น TextMeshProUGUI ---
    // การอ้างอิงไปยัง Text Component ใน Unity UI (TMP)
    public TextMeshProUGUI woodCountText;
    public TextMeshProUGUI potionCountText;
    public TextMeshProUGUI gunCountText;

    // Dictionary เพื่อเก็บชื่อไอเท็มและจำนวนที่เก็บได้
    private Dictionary<string, int> items = new Dictionary<string, int>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // กำหนดค่าเริ่มต้น
        items.Add("Wood", 0);
        items.Add("Potion", 0);
        items.Add("Gun", 0);

        UpdateAllUI();
    }

    // ฟังก์ชันสำหรับเพิ่มไอเท็ม (เหมือนเดิม)
    public void AddItem(string itemName, int amount)
    {
        if (items.ContainsKey(itemName))
        {
            items[itemName] += amount;
            UpdateUI(itemName);
        }
    }

    // ฟังก์ชันสำหรับอัปเดต UI ของไอเท็มที่กำหนด
    private void UpdateUI(string itemName)
    {
        // อัปเดต Text Component ที่เกี่ยวข้อง
        switch (itemName)
        {
            case "Wood":
                if (woodCountText != null)
                    // ใช้ .text ในการอัปเดตข้อความ
                    woodCountText.text = $"Wood: {items["Wood"]}";
                break;
            case "Potion":
                if (potionCountText != null)
                    potionCountText.text = $"Potion: {items["Potion"]}";
                break;
            case "Gun":
                if (gunCountText != null)
                    gunCountText.text = $"Gun: {items["Gun"]}";
                break;
        }
    }

    private void UpdateAllUI()
    {
        UpdateUI("Wood");
        UpdateUI("Potion");
        UpdateUI("Gun");
    }
    
}