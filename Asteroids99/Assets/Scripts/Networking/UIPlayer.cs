using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utils;

namespace Networking{
    public class UIPlayer : MonoBehaviour
    {
        [SerializeField] public TMP_Text text;
        Player player;
        // Start is called before the first frame update

        public void SetPlayer(Player player)
        {
            this.player = player;
            this.LogLog($"Set Player {player}");
            text.text = player.playerName;

        }
    }
}