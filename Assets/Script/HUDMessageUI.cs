using UnityEngine;
using TMPro;
using System.Collections;

public class HUDMessageUI : MonoBehaviour
{
    public static HUDMessageUI Instance;

    public TMP_Text messageText;
    public float showTime = 2f;

    Coroutine currentRoutine;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        gameObject.SetActive(false);
    }

    public static void Log(string msg)
    {
        if (Instance != null)
            Instance.ShowMessage(msg);
        else
            Debug.Log(msg);
    }

    public void ShowMessage(string msg)
    {
        if (messageText != null)
            messageText.text = msg;

        gameObject.SetActive(true);

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(HideAfterDelay());
    }

    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(showTime);
        gameObject.SetActive(false);
    }
}

