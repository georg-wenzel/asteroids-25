using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Security.Cryptography;
using Utils;

namespace Networking
{

    [System.Serializable]
    public class Match
    {
        public string matchID;
        public bool matchFull;
        public bool inMatch;
        public bool gameWon = false;

        private SortedSet<int> indices = new SortedSet<int>() {
                                1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25};

        public int getAndRemoveNextFreeIndex(){
            int nextIndex = indices.Min;
            indices.Remove(indices.Min);
            return nextIndex;
        }

        public void addIndex(int index)
        {
            indices.Add(index);
        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
        }

        public List<Player> GetPlayers()
        {
            return players;
        }

        public void GameWon()
        {
            gameWon = true;
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
                this.LogLog("Trying to get a match, but there are no matches in the match-list of the server-matchMaker.");
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
                this.LogLog($"Match ID {matchID} already exists");
                return false;
            }
            else
            {
                Match m = new Match(matchID, player);
                matches.Add(m);
                matchIDs.Add(matchID);
                this.LogLog("Created new Match with ID " + getMatch(matchID).matchID);
                // player.currentMatch = getMatch(matchID);
                player.matchID = matchID;
                this.LogLog ($"Match generated");
                this.LogLog($"Match: {matchID} added");
                playerIndex = getMatch(matchID).getAndRemoveNextFreeIndex();
                player.playerIndex = playerIndex;
                return true;
            }
        }

        public bool JoinGame(String matchID, Player player, out int playerIndex)
        {
            playerIndex = -1;
            if (!matchIDs.Contains(matchID))
            {
                this.LogLog($"Match ID {matchID} does not exist");
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
                        this.LogLog($"Player {player.playerName} joined match {matchID}");
                        //playerIndex = matches[i].GetPlayers().Count;
                        //player.playerIndex = playerIndex;
                        player.playerIndex = matches[i].getAndRemoveNextFreeIndex();
                        matches[i].players[0].PlayerCountUpdated (matches[i].players.Count);
                        if (matches[i].players.Count == maxMatchPlayers) {
                                matches[i].matchFull = true;
                            }
                        
                        this.LogLog ($"Match joined");
                        return true;
                    }
                }
                this.LogLog($"Could not find Match: {matchID}");
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
                    if (matches[i].players.Count > playerIndex)
                    {
                        matches[i].players.RemoveAt (playerIndex);
                        matches[i].addIndex(player.playerIndex); // make this index available again
                    }
                    this.LogLog ($"Player disconnected from match {_matchID} | {matches[i].players.Count} players remaining");

                    if (matches[i].players.Count == 0) {
                        this.LogLog ($"No more players in Match. Terminating {_matchID}");
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
