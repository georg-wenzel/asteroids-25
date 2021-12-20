using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Networking{
    public class UIPlayer : MonoBehaviour
    {
        [SerializeField] public TMP_Text text;
        Player player;
        // Start is called before the first frame update

        public void SetPlayer(Player player)
        {
            this.player = player;
            Debug.Log($"Set Player {player}");
            text.text = player.playerName;

        }
    }
}