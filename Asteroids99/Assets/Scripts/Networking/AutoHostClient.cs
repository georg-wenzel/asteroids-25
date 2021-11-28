using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class AutoHostClient : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;
    void Start(){
        if(!Application.isBatchMode){
            Debug.Log($"Client Build");
                //networkManager.StartClient();
        } else {
            Debug.Log($"Server Starting");
        }
    }

    
    
    public void JoinLocal()
    {
        Debug.Log($"Joining on {networkManager.networkAddress}");

        networkManager.StartClient();
    }
}
