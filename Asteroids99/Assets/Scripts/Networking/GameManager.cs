using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


namespace Networking
{

    public class GameManager : NetworkBehaviour
    {
        NetworkMatch networkMatchChecker;
        List<Player> players = new List<Player>();
        public void AddPlayer(Player player)
        {
            players.Add(player);

        }
    }
}
