using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class AutoHostClient : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;
    void Start(){
        // headless build
        if(!Application.isBatchMode){
            Debug.Log($"Client Build");
            networkManager.StartClient();
        } else {
            Debug.Log($"Server Starting");
        }
    }
    public void JoinLocal()
    {
        networkManager.networkAddress = "localhost";
        networkManager.StartClient();
    }
}
