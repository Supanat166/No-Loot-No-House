using UnityEngine;
using UnityEngine.SceneManagement;

public class YouWin : MonoBehaviour
{
    
    public void Retry()
    {
        Time.timeScale = 1f;  
        SceneManager.LoadScene("Main Scene");   
    }

    
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");     
    }
}
