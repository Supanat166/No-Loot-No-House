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
        // À“ Character µ—«·√°„π©“°
        player = FindObjectOfType<Character>();
    }

    void Update()
    {
        if (player == null) return;

        if (woodText != null)
            woodText.text = "Wood: " + player.woodPlanks;

        if (potionText != null)
            potionText.text = "Potion: " + player.potionCount;

        int ammo = 0;
        if (player.currentWeapon != null)
            ammo = player.currentWeapon.currentAmmo;

        if (gunText != null)
            gunText.text = "Gun: " + ammo;
    }
}

