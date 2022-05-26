using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    bool isPaused;
    public GameObject PauseMenuCanvas;
    public GameObject restartButton;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Tab))
        {
            PauseGame();
        }

        if (restartButton != null)
        if (SceneManager.GetActiveScene().name != "Level 1" && !restartButton.activeInHierarchy)
        restartButton.SetActive(true);
    }
    
    public void PauseGame()
    {
        isPaused = !isPaused;

        PauseMenuCanvas.SetActive(isPaused);
        
        Time.timeScale = isPaused? 0 : 1;
        Cursor.lockState = isPaused? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void RestartGame()
    {
        if (SceneManager.GetActiveScene().name != "Level 1")
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        GameObject.FindWithTag("Player").transform.position = GameObject.Find("Level Transition Entrance").transform.GetChild(1).position;
        
        PauseGame();
    }

    public void Exit()
    {
        PauseGame();
        Destroy(GameObject.Find("Don'tDestroyOnLoad"));
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Level 1");
    }
}
