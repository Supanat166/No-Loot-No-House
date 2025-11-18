using UnityEngine;
using UnityEngine.SceneManagement;

public class YouWin : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // ปุ่ม RETRY – กลับไปเล่นฉากเกมเดิม
    public void Retry()
    {
        Time.timeScale = 1f;  // กันไว้ เผื่อฉากก่อนหน้าเคย Pause
        SceneManager.LoadScene("Main Scene");   // ชื่อฉากเกมของคุณ
    }

    // ปุ่ม MAIN MENU – กลับหน้าเมนูหลัก
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");     // ชื่อฉากเมนูของคุณ
    }
}
