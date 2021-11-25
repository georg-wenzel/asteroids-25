using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Networking{

    public class UILobby : MonoBehaviour
    {
        public static UILobby instance;

        [Header("Host Join")]

        [SerializeField] InputField joinMatchInput;
        [SerializeField] Button joinButton;
        [SerializeField] Button hostButton;

        [SerializeField] GameObject hostJoin;
        [SerializeField] Canvas lobbyCanvas;

        [Header("Lobby")]
        [SerializeField] Transform UIPlayerParent;
        [SerializeField] GameObject UIPlayerPrefab;

        [SerializeField] TMP_Text matchIDText;
        [SerializeField] GameObject startGameButton;

        public void Host()
        {
            Debug.Log($"Player {Player.localPlayer}");
            Player.localPlayer.HostGame();

        }
        public void Join()
        {
            Player.localPlayer.JoinGame(joinMatchInput.text);
        }
        public void JoinSuccess(bool success, string matchID)
        {
            if(!success)
            {
                hostJoin.SetActive(true);
            } else {
                hostJoin.SetActive(false);
                lobbyCanvas.enabled = true;
                SpawnPlayerUIPrefab(Player.localPlayer);
                matchIDText.text = matchID;
            }

        }

        public void HostSuccess(bool success, string matchID){
            if(!success){
                hostJoin.SetActive(true);
            } else {
                hostJoin.SetActive(false);
                lobbyCanvas.enabled = true;
                SpawnPlayerUIPrefab(Player.localPlayer);
                matchIDText.text = matchID;
                startGameButton.SetActive(true);
            }
        }

        public void backToJoinHost()
        {
            // when back button in lobby is pressed
            lobbyCanvas.enabled = false;
            hostJoin.SetActive(true);
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

        public void SpawnPlayerUIPrefab(Player player) {
            GameObject newUIPlayer = Instantiate(UIPlayerPrefab, UIPlayerParent);
            newUIPlayer.GetComponent<UIPlayer>().SetPlayer(player);
        }

        public void BeginGame() {
            Player.localPlayer.BeginGame();
        }
    }
}