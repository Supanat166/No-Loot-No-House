using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LootUI : MonoBehaviour
{
    public static LootUI Instance;

    [Header("UI")]
    public TMP_Text lootText;     
    public Image lootIcon;        
    public float showTime = 2f;   

    private Coroutine showRoutine;

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        gameObject.SetActive(false); 
    }

    public void ShowLoot(LootItemData item)
    {
        if (item == null) return;

        gameObject.SetActive(true);

        
        if (lootText != null)
        {
            lootText.text = "You got: " + item.displayName;
        }

        
        if (lootIcon != null)
        {
            if (item.icon != null)
            {
                lootIcon.enabled = true;
                lootIcon.sprite = item.icon;
            }
            else
            {
                lootIcon.enabled = false;
            }
        }

        
        if (showRoutine != null)
        {
            StopCoroutine(showRoutine);
        }
        showRoutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(showTime);
        gameObject.SetActive(false);
    }
}