using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

namespace Networking
{
    [RequireComponent (typeof (NetworkMatch))]
    [System.Serializable]
    public class Player : NetworkBehaviour
    {

        #region fields
        public static Player localPlayer;
        [SyncVar] public string matchID;
        [SyncVar] public int playerIndex;

        //[SyncVar] public Match currentMatch;

        [SyncVar] public GameState gameState;

        NetworkMatch networkMatchChecker;

        [SerializeField] public GameObject playerLobbyUI;

        Guid netIDGuid;

        #endregion fields

        // Methods executed on client
        // #################################################################
        #region executedOnClient

        #region executedByClient

        void Awake () {
            networkMatchChecker = GetComponent<NetworkMatch>();
        }

        void Start()
        {
            gameState = new GameState(false, 3, 0); // initialize GameState. Will be replaced as soon as gameState gets updated
        }

        public void SetGameState(GameState gameState)
        {
            SetGameStateOnServer(gameState.GameOver, gameState.HP, gameState.Score); // set GameState on Server
        }

        public void HostGame()
        {
            string matchID = MatchMaker.GetRandomMatchID();
            CmdHostGame(matchID);
        }

        public void SpawnAttackAsteroid(Player targetPlayer)
        {
            CmdSpawnAsteroid(targetPlayer);
        }

        public void JoinGame(string inputID)
        {
            CmdJoinGame(inputID);
        }

        public void BeginGame()
        {
            CmdBeginGame();
        }

        public void PrintClientDataOfMatch()
        {
            Debug.Log("#################");
            Debug.Log("this: " + this);
            Debug.Log("this.playerIndex: " + this.playerIndex);
            Debug.Log("Players in match:");
            // foreach(Player p in this.currentMatch.players)
            // {
            //     Debug.Log("-----");
            //     Debug.Log("Player Index: " + p.playerIndex);
            //     Debug.Log("Player Score: " + p.gameState.Score);
            //     Debug.Log("-----");
            // }
            Debug.Log("#################");
        }

        #endregion executedByClient

        #region executedByServer

        [TargetRpc]
        void TargetHostGame(bool success, string matchID, int playerIndex)
        {
            //localPlayer.playerIndex = playerIndex;
            Debug.Log($"This MatchId: {matchID}");
            UILobby.instance.HostSuccess(success, matchID);
        }

        [TargetRpc]
        void TargetJoinGame(bool success, string matchID, int playerIndex)
        {
            //localPlayer.playerIndex = playerIndex;
            //localPlayer.matchID = matchID;
            Debug.Log($"This MatchId: {matchID}");
            UILobby.instance.JoinSuccess(success, matchID);
        }

        [TargetRpc]
        void TargetBeginGame()
        {
            Debug.Log($"This MatchId: {matchID} | Starting Game");
            //Additively LoadGameScene
            NetworkHelper helper = new GameObject("NetworkHelper").AddComponent<NetworkHelper>();
            StartCoroutine(helper.LoadSceneEnumerator("OnlineGameScene"));
            UILobby.instance.startSuccess();
            Debug.Log("Loaded Game");
        }

        [TargetRpc]
        public void UpdateGUIOnClients(List<GameState> enemyGameStates)
        {
            // PrintClientDataOfMatch();
            GameObject.Find("EnemyBoxes").GetComponent<EnemyBoxes>().UpdateEnemySquares(enemyGameStates);
        }

        [TargetRpc]
        void TargetSpawnAsteroid(bool success, string matchID, int playerIndex)
        {
            //TODO
            GameObject.Find("AsteroidManager").GetComponent<AsteroidSpawner>().SpawnAttackAsteroid();
        }

        #endregion executedByServer

        #endregion executedOnClient

        // Methods executed on server
        // #################################################################
        #region executedOnServer

        // public void PrintServerDataOfMatch()
        // {
        //     Debug.Log("#################");
        //     Debug.Log("this: " + this);
        //     Debug.Log("this.playerIndex: " + this.playerIndex);
        //     Debug.Log("Players in match:");
        //     foreach(Player p in this.currentMatch.players)
        //     {
        //         Debug.Log("-----");
        //         Debug.Log("Player Index: " + p.playerIndex);
        //         Debug.Log("Player Score: " + p.gameState.Score);
        //         Debug.Log("-----");
        //     }
        //     Debug.Log("#################");
        // }

        public void UpdateGameStatesOnClients()
        {
            foreach(Player pl in MatchMaker.instance.getMatch(matchID).players)
            {
                List<GameState> gameStates = new List<GameState>();
                foreach(Player p in MatchMaker.instance.getMatch(matchID).players)
                {
                    gameStates.Add(p.gameState);
                }
                for(int i=MatchMaker.instance.getMatch(matchID).players.Count; i<25; i++)
                {
                    gameStates.Add(new GameState(true, 0, 0)); // dummy gameState which is GameOver
                }
                gameStates.Remove(pl.gameState);
                pl.UpdateGUIOnClients(gameStates);
            }
        }

        [Command]
        public void SetGameStateOnServer(bool GameOver, int HP, int Score)
        {
            GameState gs = new GameState(GameOver, HP, Score);
            this.gameState = gs;
            UpdateGameStatesOnClients();
        }

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

        [Command]
        void CmdBeginGame()
        {
            MatchMaker.instance.BeginGame(matchID);
            Debug.Log($"Starting Game");
        }

        public void StartGame() // called by Server from MatchMaker - therefore runs on Server
        {
            TargetBeginGame();
        }

        [Command]
        void CmdSpawnAsteroid(Player targetPlayer)
        {
            TargetSpawnAsteroid(true, matchID, targetPlayer.playerIndex);
        }

        #endregion executedOnServer

        // #################################################################
        #region ServerClientStartStop

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

        #endregion ServerClientStartStop

        // #################################################################
        #region disconnect

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

        #endregion disconnect

        // #################################################################
        #region matchPlayers
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
        public void TargetPlayerFillLobby (Player player) 
        {
            playerLobbyUI = UILobby.instance.SpawnPlayerUIPrefab (this);
        }

        #endregion matchPlayers

    }
}
