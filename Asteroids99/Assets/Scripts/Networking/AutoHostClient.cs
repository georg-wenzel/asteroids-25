using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using TMPro;

public class AutoHostClient : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;
    [SerializeField] TMP_Text warningText;
    void Start(){
        if(!Application.isBatchMode){
            Debug.Log($"Client Build");
                //networkManager.StartClient();
        } else {
            Debug.Log($"Server Starting");
        }
    }

    public void SetNetworkOnLocalHost()
    {
        networkManager.networkAddress = "localhost";
    }

    public void SetNetworkOnGameServer()
    {
        networkManager.networkAddress = "3.65.169.108";
    }
    
    public void JoinLocal()
    {
        Debug.Log($"Joining on {networkManager.networkAddress}");
        warningText.enabled = true;
        networkManager.StartClient();
    }
}
