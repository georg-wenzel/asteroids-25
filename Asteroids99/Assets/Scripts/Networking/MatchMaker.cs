using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Security.Cryptography;

namespace Networking
{

    [System.Serializable]
    public class Match
    {
        public string matchID;

        public void AddPlayer(GameObject player)
        {
            players.Add(player);
        }

        public SyncList<GameObject> GetPlayers()
        {
            return players;
        }

        readonly SyncList<GameObject> players = new SyncList<GameObject>();

        public Match(string matchID, GameObject player)
        {
            this.matchID = matchID;
            players.Add(player);
        }
        public Match() { }

    }

    public class MatchMaker : NetworkBehaviour
    {

        public static MatchMaker instance;

        readonly SyncList<Match> matches = new SyncList<Match>();
        readonly SyncList<string> matchIDs = new SyncList<string>();

        [SerializeField] GameObject gameManagerPrefab;
        public bool HostGame(string matchID, GameObject player, out int playerIndex)
        {
            if (matchIDs.Contains(matchID))
            {
                Debug.Log($"Match ID {matchID} already exists");
                playerIndex = -1;
                return false;
            }
            else
            {
                matches.Add(new Match(matchID, player));
                matchIDs.Add(matchID);
                Debug.Log($"Match: {matchID} added");
                playerIndex = 1;
                return true;
            }

        }
        public bool JoinGame(string matchID, GameObject player, out int playerIndex)
        {
            playerIndex = -1;
            if (!matchIDs.Contains(matchID))
            {
                Debug.Log($"Match ID {matchID} does not exist");
                return false;
            }
            else
            {
                for (int i = 0; i < matches.Count; i++)
                {
                    if (matches[i].matchID == matchID)
                    {
                        matches[i].AddPlayer(player);
                        Debug.Log($"Player {player.name} joined match {matchID}");
                        playerIndex = matches[i].GetPlayers().Count;
                        return true;
                    }
                }
                Debug.Log($"Could not find Match: {matchID}");
                return false;
            }
        }
        public void BeginGame(String matchID)
        {
            GameObject newGameManager = Instantiate(gameManagerPrefab);
            NetworkServer.Spawn(newGameManager);
            newGameManager.GetComponent<NetworkMatch>().matchId = matchID.toGuid();
            GameManager gameManager = newGameManager.GetComponent<GameManager>();
            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i].matchID == matchID)
                {
                    foreach(var player in matches[i].GetPlayers())
                    {
                     Player _player = player.GetComponent<Player>();  
                     gameManager.AddPlayer(_player);
                     _player.StartGame();
                    }

                }
            }
        }
        public static string GetRandomMatchID()
        {
            string _id = string.Empty;
            for (int i = 0; i < 7; i++)
            {
                _id += UnityEngine.Random.Range(0, 9).ToString();
            }
            Debug.Log($"Random Match ID: {_id}");
            return _id;

        }
        // Start is called before the first frame update
        void Start()
        {
            instance = this;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
    public static class MatchExtensions
    {
        public static Guid toGuid(this string matchID)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(matchID);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            return new Guid(hashBytes);
        }
    }
}