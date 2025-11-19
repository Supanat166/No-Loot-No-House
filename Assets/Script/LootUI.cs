using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LootUI : MonoBehaviour
{
    public static LootUI Instance;

    [Header("UI")]
    public TMP_Text lootText;     // ข้อความ "You got: xxx"
    public Image lootIcon;        // รูปไอเท็ม (ไม่ใช้ก็ปล่อยว่างได้)
    public float showTime = 2f;   // แสดงกี่วินาทีแล้วค่อยหาย

    private Coroutine showRoutine;

    private void Awake()
    {
        // ทำเป็น Singleton แบบง่าย ๆ
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        gameObject.SetActive(false); // เริ่มเกมซ่อนไว้ก่อน
    }

    public void ShowLoot(LootItemData item)
    {
        if (item == null) return;

        gameObject.SetActive(true);

        // ข้อความ
        if (lootText != null)
        {
            lootText.text = "You got: " + item.displayName;
        }

        // ไอคอน
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

        // ถ้าเคยมี coroutine เก่าอยู่ ให้หยุดก่อน
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

