using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using TMPro;
using Utils;

public class AutoHostClient : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;
    [SerializeField] TMP_Text warningText;
    void Start(){
        if(!Application.isBatchMode){
            this.LogLog($"Client Build");
                //networkManager.StartClient();
        } else {
            this.LogLog($"Server Starting");
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
        this.LogLog($"Joining on {networkManager.networkAddress}");
        warningText.enabled = true;
        networkManager.StartClient();
    }
}
