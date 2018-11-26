using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public static bool IsGamePaused = false;

    public GameObject pauseMenuUI;
    private GameObject settingsMenuUI;

    // Use this for initialization
    void Start()
    {
        settingsMenuUI = GameObject.Find("Canvas").GetComponent<SettingsMenu>().settingsMenuUI;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        IsGamePaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        IsGamePaused = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenSettingsMenu()
    {
        settingsMenuUI.SetActive(true);
        //pauseMenuUI.SetActive(false);
    }
}
