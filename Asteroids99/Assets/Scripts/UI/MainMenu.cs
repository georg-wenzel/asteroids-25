using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    #region methods

    public void PlaySinglePlayer()
    {
        // Load some Scene
        SceneManager.LoadScene("GameScene");
    }

    public void PlayMultiPlayer()
    {
        // Load some Scene
        SceneManager.LoadScene("MultiplayerConnect");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion
}
