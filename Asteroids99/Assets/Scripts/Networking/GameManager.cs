using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Utils;


namespace Networking
{

    public class GameManager : NetworkBehaviour
    {
        NetworkMatch networkMatchChecker;
        List<Player> players = new List<Player>();
        public void AddPlayer(Player player)
        {
            this.LogLog($"Adding player {player} {player.playerIndex }");
            players.Add(player);

        }
    }
}
