using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class AutoHostClient : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;
    void Start(){
        if(!Application.isBatchMode){
            Debug.Log($"Client Build");
            Debug.Log($"NetworkAdress: {networkManager.networkAddress}");
            //networkManager.StartHost();
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
