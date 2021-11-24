using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Networking{
    public class UIPlayer : MonoBehaviour
    {
        [SerializeField] Text text;
        Player player;
        // Start is called before the first frame update

        public void SetPlayer(Player player)
        {
            this.player = player;
            Debug.Log($"Set Player {player}");
            text.text = "Player " + player.playerIndex.ToString();

        }
    }
}