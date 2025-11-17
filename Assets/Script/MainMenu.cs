using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        SceneManager.LoadScene("Main Scene");   // ชื่อฉากเกมของคุณ
    }

    // เปิดหน้า Options
    public void OpenOptions()
    {
        // ถ้ามีหน้า Options เป็น Canvas ก็เปิดปิดได้แบบนี้
        // optionsUI.SetActive(true);

        //SceneManager.LoadScene("OptionsScene"); // ถ้าคุณทำเป็นฉากแยก
    }

    // ออกจากเกม
    public void QuitGame()
    {
        Debug.Log("Quit Game!");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
