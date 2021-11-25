using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerUI : MonoBehaviour
{
    public void goToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void goToMultiplayerConnect()
    {
        SceneManager.LoadScene("MultiplayerConnect");
    }
}
