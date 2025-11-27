using UnityEngine;
using TMPro; // �Ӥѭ: ��ͧ�� namespace �������Ѻ TextMeshPro
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    // Singleton Pattern 
    public static InventoryManager Instance;

    
    public TextMeshProUGUI woodCountText;
    public TextMeshProUGUI potionCountText;
    public TextMeshProUGUI gunCountText;

    
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

        
        items.Add("Wood", 0);
        items.Add("Potion", 0);
        items.Add("Gun", 0);

        UpdateAllUI();
    }

    
    public void AddItem(string itemName, int amount)
    {
        if (items.ContainsKey(itemName))
        {
            items[itemName] += amount;
            UpdateUI(itemName);
        }
    }

    
    private void UpdateUI(string itemName)
    {
        
        switch (itemName)
        {
            case "Wood":
                if (woodCountText != null)
                    
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