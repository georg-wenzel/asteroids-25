using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Networking;

public class MultiplayerUI : MonoBehaviour
{
    [SerializeField] GameObject hostJoin;
    [SerializeField] TMP_InputField nameInput;

    public void goToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void goToMultiplayerConnect()
    {
        SceneManager.LoadScene("MultiplayerConnect");
    }

}
