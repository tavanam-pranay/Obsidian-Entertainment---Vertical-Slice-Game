using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuParent;

    public void openPauseMenu()
    {
        if (pauseMenuParent.activeInHierarchy) return; // if the menu is already open, do nothing
        pauseMenuParent.SetActive(true);
        Time.timeScale = 0f;
    }

    public void closePauseMenu()
    {
        if (!pauseMenuParent.activeInHierarchy) return; // if the menu is already closed, do nothing
        pauseMenuParent.SetActive(false);
        Time.timeScale = 1f;
    }

    public void openMainMenu()
    {
        Time.timeScale = 1f; // make sure time is running when we go back to the main menu
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); // load the main menu scene (assuming it's at index 0)
    }
}