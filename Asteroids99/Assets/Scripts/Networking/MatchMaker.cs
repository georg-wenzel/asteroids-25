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
        public bool matchFull;
        public bool inMatch;

        public void AddPlayer(Player player)
        {
            players.Add(player);
        }

        public List<Player> GetPlayers()
        {
            return players;
        }

        public List<Player> players = new List<Player> ();
        public Match(string matchID, Player player)
        {
            this.matchID = matchID;
            inMatch = false;
            matchFull = false;
            players.Add(player);
        }
        public Match() { }

    }

    public class MatchMaker : NetworkBehaviour
    {

        public static MatchMaker instance;

        readonly SyncList<Match> matches = new SyncList<Match>();
        readonly SyncList<String> matchIDs = new SyncList<String>();

        [SerializeField] GameObject gameManagerPrefab;
        [SerializeField] int maxMatchPlayers = 25;
        public Match getMatch(String matchID) {
            if(matches.Count == 0)
                Debug.Log("Trying to get a match, but there are no matches in the match-list of the server-matchMaker.");
            foreach (Match match in matches)
            {
                if (match.matchID == matchID)
                {
                    return match;
                }
            }
            return null;
        }
        public bool HostGame(String matchID, Player player, out int playerIndex)
        {
            playerIndex = -1;
            if (matchIDs.Contains(matchID))
            {
                Debug.Log($"Match ID {matchID} already exists");
                return false;
            }
            else
            {
                Match m = new Match(matchID, player);
                matches.Add(m);
                matchIDs.Add(matchID);
                Debug.Log("Created new Match with ID " + getMatch(matchID).matchID);
                // player.currentMatch = getMatch(matchID);
                player.matchID = matchID;
                Debug.Log ($"Match generated");
                Debug.Log($"Match: {matchID} added");
                playerIndex = 1;
                player.playerIndex = playerIndex;
                return true;
            }
        }

        public bool JoinGame(String matchID, Player player, out int playerIndex)
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
                        // player.currentMatch = matches[i];
                        player.matchID = matchID;
                        Debug.Log($"Player {player.playerName} joined match {matchID}");
                        playerIndex = matches[i].GetPlayers().Count;
                        player.playerIndex = playerIndex;
                        matches[i].players[0].PlayerCountUpdated (matches[i].players.Count);
                        if (matches[i].players.Count == maxMatchPlayers) {
                                matches[i].matchFull = true;
                            }
                        
                        Debug.Log ($"Match joined");
                        return true;
                    }
                }
                Debug.Log($"Could not find Match: {matchID}");
                return false;
            }
        }
        
        public void BeginGame(String matchID)
        {
            //GameManager gameManager = Instantiate (gameManagerPrefab).GetComponent<GameManager> ();
            for (int i = 0; i < matches.Count; i++) {
                if (matches[i].matchID == matchID) {
                    matches[i].inMatch = true;
                    foreach (var player in matches[i].players) {
                        //gameManager.AddPlayer(player);
                        player.StartGame ();
                    }
                    break;
                }
            }
        }
        public void PlayerDisconnected (Player player, string _matchID) {
            for (int i = 0; i < matches.Count; i++) {
                if (matches[i].matchID == _matchID) {
                    int playerIndex = matches[i].players.IndexOf (player);
                    if (matches[i].players.Count > playerIndex) matches[i].players.RemoveAt (playerIndex);
                    Debug.Log ($"Player disconnected from match {_matchID} | {matches[i].players.Count} players remaining");

                    if (matches[i].players.Count == 0) {
                        Debug.Log ($"No more players in Match. Terminating {_matchID}");
                        matches.RemoveAt (i);
                        matchIDs.Remove (_matchID);
                    } else {
                        matches[i].players[0].PlayerCountUpdated (matches[i].players.Count);
                    }
                    break;
                }
            }
        }

        public static string GetRandomMatchID()
        {
            string _id = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                _id += UnityEngine.Random.Range(0, 9).ToString();
            }
            Debug.Log($"Random Match ID: {_id}");
            return _id;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (instance == null)
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
