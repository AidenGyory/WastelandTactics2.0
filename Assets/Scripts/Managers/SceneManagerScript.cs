using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneManagerScript : MonoBehaviour
{
    [SerializeField] string sceneName; 

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit(); 
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1.0f;
    }
}
