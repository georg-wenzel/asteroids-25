using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Networking;

public class MultiplayerUI : MonoBehaviour
{
    [SerializeField] GameObject giveName;
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

    public void AfterUserNameInserted()
    {
        if(nameInput.text != "")
        {
            // TODO: set playername

            giveName.SetActive(false);
            hostJoin.SetActive(true);
        }
    }
}
