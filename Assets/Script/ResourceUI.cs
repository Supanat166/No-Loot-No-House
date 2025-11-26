using UnityEngine;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    public TMP_Text woodText;
    public TMP_Text potionText;
    public TMP_Text gunText;

    private Character player;

    void Start()
    {
        // หา Character ตัวแรกในฉาก
        player = FindObjectOfType<Character>();

    }

    void Update()
    {
        if (player == null) return;

        // 1. ไม้: แทรกแท็กรูปภาพไม้ (สมมติว่าชื่อ "WoodIcon")
        if (woodText != null)
            // ใช้แท็ก <sprite name="ชื่อรูป">
            woodText.text = "<sprite name=\"logs\" > " + player.woodPlanks;

        // 2. ยา: แทรกแท็กรูปภาพยา (สมมติว่าชื่อ "PotionIcon")
        if (potionText != null)
            potionText.text = "<sprite name=\"bottle\"> " + player.potionCount;

        // 3. ปืน: (ใช้โค้ดเดิม แต่ถ้าอยากใส่รูปปืน ให้ใช้แท็กปืน)
        int ammo = 0;
        if (player.currentWeapon != null)
            ammo = player.currentWeapon.currentAmmo;

        if (gunText != null)
            // ใช้แท็กรูปภาพปืน (สมมติว่าชื่อ "GunIcon")
            gunText.text = "<sprite name=\"flintlock\"> " + ammo;
    }
}

