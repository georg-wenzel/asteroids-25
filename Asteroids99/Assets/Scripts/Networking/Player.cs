using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System.Linq;
using Utils;

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

        [SyncVar] public string playerName; 
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

        public void HostGame(string name)
        {
            string matchID = MatchMaker.GetRandomMatchID();
            CmdHostGame(matchID, name);
        }

        public void SpawnAttackAsteroid(int targetPlayerIndex)
        {
            CmdSpawnAsteroid(targetPlayerIndex);
        }

        public void JoinGame(string inputID, string name)
        {
            CmdJoinGame(inputID, name);
        }

        public void BeginGame()
        {
            CmdBeginGame();
        }

        public void PrintClientDataOfMatch()
        {
            this.LogLog("#################");
            this.LogLog("this: " + this);
            this.LogLog("this.playerIndex: " + this.playerIndex);
            this.LogLog("Players in match:");
            // foreach(Player p in this.currentMatch.players)
            // {
            //     Debug.Log("-----");
            //     Debug.Log("Player Index: " + p.playerIndex);
            //     Debug.Log("Player Score: " + p.gameState.Score);
            //     Debug.Log("-----");
            // }
            this.LogLog("#################");
        }

        #endregion executedByClient

        #region executedByServer

        [TargetRpc]
        void TargetHostGame(bool success, string matchID, int playerIndex)
        {
            //localPlayer.playerIndex = playerIndex;
            this.LogLog($"This MatchId: {matchID}");
            UILobby.instance.HostSuccess(success, matchID);
        }

        [TargetRpc]
        void TargetJoinGame(bool success, string matchID, int playerIndex)
        {
            //localPlayer.playerIndex = playerIndex;
            //localPlayer.matchID = matchID;
            this.LogLog($"This MatchId: {matchID}");
            UILobby.instance.JoinSuccess(success, matchID);
        }

        [TargetRpc]
        void TargetBeginGame()
        {
            this.LogLog($"This MatchId: {matchID} | Starting Game");
            //Additively LoadGameScene
            NetworkHelper helper = new GameObject("NetworkHelper").AddComponent<NetworkHelper>();
            StartCoroutine(helper.LoadSceneEnumerator("OnlineGameScene"));
            UILobby.instance.startSuccess();
            this.LogLog("Loaded Game");
        }

        [TargetRpc]
        public void UpdateGUIOnClients(List<GameState> enemyGameStates, List<string> enemyNames, List<int> playerIndices)
        {
            // PrintClientDataOfMatch();
            GameObject.Find("EnemyBoxes").GetComponent<EnemyBoxes>().UpdateEnemySquares(enemyGameStates, enemyNames, playerIndices);
            bool allEnemiesGameOver = true;
            foreach(GameState gs in enemyGameStates)
            {
                if (!gs.GameOver)
                {
                    allEnemiesGameOver = false;
                    break;
                }
            }
            if(allEnemiesGameOver)
            {
                GameObject.Find("HUD").GetComponent<GameHUD>().GameWon();
            }
        }

        [TargetRpc]
        void TargetSpawnAsteroid()
        {
            this.LogLog("Enemy spawned an AttackAsteroid on this player");
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

        // run on server
        public void UpdateGameStatesOnClients()
        {
            if (MatchMaker.instance.getMatch(matchID).players.Count > 1)
            {
                foreach (Player pl in MatchMaker.instance.getMatch(matchID).players)
                {
                    List<GameState> gameStates = new List<GameState>();
                    List<string> playerNames = new List<string>();
                    List<int> playerIndices = new List<int>();
                    foreach (Player p in MatchMaker.instance.getMatch(matchID).players)
                    {
                        gameStates.Add(p.gameState);
                        playerNames.Add(p.playerName);
                        playerIndices.Add(p.playerIndex);
                    }

                    for (int i = MatchMaker.instance.getMatch(matchID).players.Count; i < 25; i++)
                    {
                        gameStates.Add(new GameState(true, 0, 0)); // dummy gameState which is GameOver
                        playerNames.Add("Player " + i);
                        playerIndices.Add(playerIndices.Last() + 1);
                    }

                    gameStates.Remove(pl.gameState);
                    var index = playerNames.IndexOf(pl.playerName);
                    playerNames.RemoveAt(index);
                    playerIndices.Remove(pl.playerIndex);
                    pl.UpdateGUIOnClients(gameStates, playerNames, playerIndices);

                    bool allGameOver = true;
                    foreach(GameState gs in gameStates)
                    {
                        if(!gs.GameOver)
                            allGameOver = false;
                    }
                    if(allGameOver)
                    {
                        MatchMaker.instance.getMatch(matchID).GameWon();
                        // save player data to database
                        // get player data with MatchMaker.instance.getMatch(matchID).players
                        // iterate over the above list and call player.playerName and player.gameState.score
                    }
                }
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
        void CmdHostGame(string matchID, string name)
        {
            this.matchID = matchID;
            this.playerName = name;
            if (MatchMaker.instance.HostGame(matchID, this, out playerIndex))
            {
                this.LogLog("Success Hosting game " + matchID);
                networkMatchChecker.matchId = matchID.toGuid();
                TargetHostGame(true, matchID, playerIndex);
            }
            else
            {
                this.LogWarn($"<color=red>Failed to host game </color>");
                TargetHostGame(false, matchID, playerIndex);
            }
        }

        [Command]
        void CmdJoinGame(string matchID, string name)
        {
            this.matchID = matchID;
            this.playerName = name;
            if (MatchMaker.instance.JoinGame(matchID, this, out playerIndex))
            {
                this.LogLog("Success Joining game " + matchID);
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
            this.LogLog($"Starting Game");
        }

        public void StartGame() // called by Server from MatchMaker - therefore runs on Server
        {
            TargetBeginGame();
        }

        [Command]
        void CmdSpawnAsteroid(int targetPlayerIndex)
        {
            foreach (Player p in MatchMaker.instance.getMatch(matchID).players)
            {
                if (p.playerIndex == targetPlayerIndex)
                {
                    p.TargetSpawnAsteroid();
                    break;
                }
            }
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
                this.LogLog ($"Spawning other player UI Prefab");
                playerLobbyUI = UILobby.instance.SpawnPlayerUIPrefab (this);
            }
        }

        public override void OnStopClient () {
            this.LogLog ($"Client Stopped");
            ClientDisconnect ();
        }

        public override void OnStopServer () {
            this.LogLog ($"Client Stopped on Server");
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

        #endregion matchPlayers

    }
}
