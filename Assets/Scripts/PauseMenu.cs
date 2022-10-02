using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameEngine gameEngine;

    // Start is called before the first frame update
    void Start()
    {
        // Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseMenu.activeSelf)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    public void Quit()
    {
        // Application.Quit();
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        // Cursor.visible = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        // Cursor.visible = false;
    }

    public void Restart()
    {
        // gameEngine.Restart();
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
