using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

namespace Networking
{
        [RequireComponent (typeof (NetworkMatch))]
        public class Player : NetworkBehaviour
    {
    
        public static Player localPlayer;
        [SyncVar] public string matchID;
        [SyncVar] public int playerIndex;

        [SyncVar] public Match currentMatch;

        NetworkMatch networkMatchChecker;

        [SerializeField] public GameObject playerLobbyUI;

        Guid netIDGuid;


        void Awake () {
            networkMatchChecker = GetComponent<NetworkMatch> ();
        }

        public override void OnStartServer () {
            netIDGuid = netId.ToString ().toGuid();
            networkMatchChecker.matchId = netIDGuid;
        }


        public override void OnStartClient () {
            if (isLocalPlayer) {
                localPlayer = this;
            } else {
                Debug.Log ($"Spawning other player UI Prefab");
               
            }
        }

        public override void OnStopClient () {
            Debug.Log ($"Client Stopped");
            ClientDisconnect ();
        }

        public override void OnStopServer () {
            Debug.Log ($"Client Stopped on Server");
            ServerDisconnect ();
        }
        public void HostGame()
        {
            string matchID = MatchMaker.GetRandomMatchID();
            CmdHostGame(matchID);
        }
        // runs on server Version of Player
        [Command]
        void CmdHostGame(string matchID)
        {
            this.matchID = matchID;
            if (MatchMaker.instance.HostGame(matchID, this, out playerIndex))
            {
                Debug.Log("Success Hosting game " + matchID);
                networkMatchChecker.matchId = matchID.toGuid();
                TargetHostGame(true, matchID, playerIndex);
            }
            else
            {
                Debug.Log($"<color=red>Failed to host game </color>");
                TargetHostGame(false, matchID, playerIndex);
            }

        }

        [TargetRpc]
        void TargetHostGame(bool success, string matchID, int playerIndex)
        {
            this.playerIndex = playerIndex;
            Debug.Log($"This MatchId: {matchID}");
            UILobby.instance.HostSuccess(success, matchID);

        }



        public void JoinGame(string inputID)
        {

            CmdJoinGame(inputID);
        }
        // runs on server Version of Player
        [Command]
        void CmdJoinGame(string matchID)
        {

            this.matchID = matchID;
            if (MatchMaker.instance.JoinGame(matchID, this, out playerIndex))
            {

                Debug.Log("Success Joining game " + matchID);
                 playerLobbyUI = UILobby.instance.SpawnPlayerUIPrefab (this);
                networkMatchChecker.matchId = matchID.toGuid();
                TargetJoinGame(true, matchID, playerIndex);
            }
            else
            {
                Debug.Log($"<color=red>Failed to Join game </color");
                TargetJoinGame(false, matchID, playerIndex);
            }

        }

        [TargetRpc]
        void TargetJoinGame(bool success, string matchID, int playerIndex)
        {
            this.playerIndex = playerIndex;
            this.matchID = matchID;
            Debug.Log($"This MatchId: {matchID}");
            UILobby.instance.JoinSuccess(success, matchID);

        }

        public void BeginGame()
        {
            CmdBeginGame();
        }
        // runs on server Version of Player
        [Command]
        void CmdBeginGame()
        {
            MatchMaker.instance.BeginGame(matchID);
            Debug.Log($"Starting Game");

        }

        public void StartGame()
        {
            TargetBeginGame();

        }


        [TargetRpc]
        void TargetBeginGame()
        {
            Debug.Log($"This MatchId: {matchID} | Starting Game");
            //Additively LoadGameScene
            SceneManager.LoadScene("Game", LoadSceneMode.Additive);
        }

        // Update is called once per frame
        void Update()
        {

        }

        /* 
            DISCONNECT
        */

        public void DisconnectGame () {
            CmdDisconnectGame ();
        }

        [Command]
        void CmdDisconnectGame () {
            ServerDisconnect ();
        }

        void ServerDisconnect () {
            MatchMaker.instance.PlayerDisconnected (this, matchID);
            RpcDisconnectGame ();
            networkMatchChecker.matchId = netIDGuid;
        }

        [ClientRpc]
        void RpcDisconnectGame () {
            ClientDisconnect ();
        }

        void ClientDisconnect () {
            if (playerLobbyUI != null) {
                if (!isServer) {
                    Destroy (playerLobbyUI);
                } else {
                    playerLobbyUI.SetActive (false);
                }
            }
        }

        /* 
            MATCH PLAYERS
        */

        [Server]
        public void PlayerCountUpdated (int playerCount) {
            TargetPlayerCountUpdated (playerCount);
        }

        [TargetRpc]
        void TargetPlayerCountUpdated (int playerCount) {
            if (playerCount > 1) {
                UILobby.instance.SetStartButtonActive (true);
            } else {
                UILobby.instance.SetStartButtonActive(false);
            }
        }
        

        [TargetRpc]
        public void TargetPlayerFillLobby (Player player) {
            playerLobbyUI = UILobby.instance.SpawnPlayerUIPrefab (this);
            }
        


    }
}