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
        // �� Character ����á㹩ҡ
        player = FindObjectOfType<Character>();

    }

    void Update()
    {
        if (player == null) return;
        
        if (woodText != null)
            woodText.text = "<sprite name=\"logs\" > " + player.woodPlanks;
        
        if (potionText != null)
            potionText.text = "<sprite name=\"bottle\"> " + player.potionCount;
        
        int ammo = player.totalAmmo;

        if (gunText != null)
            gunText.text = "<sprite name=\"flintlock\"> " + ammo;
    }
}

