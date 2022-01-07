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

    public void goToMainMenu()
    {
        Destroy(GameObject.Find("NetworkManager"));
        SceneManager.LoadScene("MainMenu");
    }

    public void goToMultiplayerConnect()
    {
        SceneManager.LoadScene("MultiplayerConnect");
    }

}
