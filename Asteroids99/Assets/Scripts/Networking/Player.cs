using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

namespace Networking{
    public class Player : NetworkBehaviour
    {
        public static Player localPlayer;
        [SyncVar] public string matchID;
        [SyncVar] public int playerIndex;

        NetworkMatch networkMatchChecker;



        // Start is called before the first frame update
        void Start()
        {
            networkMatchChecker = GetComponent<NetworkMatch>();
            if (isLocalPlayer)
            {
                localPlayer = this;
            }
            else
            {
                UILobby.instance.SpawnPlayerUIPrefab(this);
            }
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
            if (MatchMaker.instance.HostGame(matchID, gameObject, out playerIndex))
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
            if (MatchMaker.instance.JoinGame(matchID, gameObject, out playerIndex))
            {
                Debug.Log("Success Joining game " + matchID);
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

        public void StartGame() {
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
    }
}