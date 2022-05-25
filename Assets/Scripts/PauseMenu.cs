using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    bool isPaused;
    public GameObject PauseMenuCanvas;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Tab))
        {
            PauseGame();
        }
    }
    
    public void PauseGame()
    {
        isPaused = !isPaused;

        PauseMenuCanvas.SetActive(isPaused);
        
        Time.timeScale = isPaused? 0 : 1;
        Cursor.lockState = isPaused? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void Exit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
