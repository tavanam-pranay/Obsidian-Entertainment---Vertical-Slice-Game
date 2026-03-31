using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenu : MonoBehaviour
{
    [SerializeField] private int levelIndexToLoad;

    public AudioManager audioManager;

    public void startFirstLevel()
    {
        audioManager.PlayClick();
        Invoke("LoadLevel", audioManager.clickSound.length);
    }

    void LoadLevel()
    {
        SceneManager.LoadScene(levelIndexToLoad);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}