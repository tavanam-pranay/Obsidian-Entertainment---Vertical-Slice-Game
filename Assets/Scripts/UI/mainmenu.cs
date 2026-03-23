using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenu : MonoBehaviour
{
    [SerializeField] private int levelIndexToLoad;

    public void startFirstLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelIndexToLoad);
    }

    public void quitGame()
    {
        Application.Quit();
    }


}
