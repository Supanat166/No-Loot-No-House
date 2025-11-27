using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void StartGame()
    {
        SceneManager.LoadScene("Main Scene");   
    }

    
    public void OpenOptions()
    {
        
    }

    
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
