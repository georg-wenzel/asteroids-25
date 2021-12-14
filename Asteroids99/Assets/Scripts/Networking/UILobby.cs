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
        [SerializeField]  public GameObject startGameButton;

        GameObject localPlayerLobbyUI;


        public void Host()
        {
            Debug.Log($"Player {Player.localPlayer}");
            Player.localPlayer.HostGame();

        }
        public void Join()
        {
            Player.localPlayer.JoinGame(joinMatchInput.text);
        }
        public void SetStartButtonActive (bool active) {
            startGameButton.SetActive (active);
        }
        public void DisconnectGame () {
            if (localPlayerLobbyUI != null) Destroy (localPlayerLobbyUI);
            Player.localPlayer.DisconnectGame ();

            lobbyCanvas.enabled = false;
        }

        public void JoinSuccess(bool success, string matchID)
        {
            if(!success)
            {
                hostJoin.SetActive(true);
            } else {
                hostJoin.SetActive(false);
                lobbyCanvas.enabled = true;
                if (localPlayerLobbyUI != null) Destroy (localPlayerLobbyUI);
                localPlayerLobbyUI = SpawnPlayerUIPrefab (Player.localPlayer);
                matchIDText.text = matchID;
                startGameButton.SetActive(false);
            }
        }
        public void HostSuccess(bool success, string matchID)
        {
            if(!success){
                hostJoin.SetActive(true);
            } else {
                hostJoin.SetActive(false);
                lobbyCanvas.enabled = true;
                if (localPlayerLobbyUI != null) Destroy (localPlayerLobbyUI);
                localPlayerLobbyUI = SpawnPlayerUIPrefab (Player.localPlayer);
                matchIDText.text = matchID;
                startGameButton.SetActive(true);
            }
        }

        public void startSuccess()
        {
            Debug.Log("UI Lobby Start Success");
            hostJoin.SetActive(false);
            startGameButton.SetActive(false);
            lobbyCanvas.enabled = false;
            localPlayerLobbyUI.SetActive(false);
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

        public GameObject SpawnPlayerUIPrefab(Player player) {
            GameObject newUIPlayer = Instantiate(UIPlayerPrefab, UIPlayerParent);
            newUIPlayer.GetComponent<UIPlayer>().SetPlayer(player);
            newUIPlayer.transform.SetSiblingIndex(player.playerIndex - 1);
            return newUIPlayer;
        }

        public void BeginGame() 
        {
            Player.localPlayer.BeginGame();
        }
    }
}