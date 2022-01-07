using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

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

    public void ShowLeaderboard()
    {
        // Load some Scene
        SceneManager.LoadScene("Leaderboard");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion
}
