using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Networking{

    public class UILobby : MonoBehaviour
    {
        public static UILobby instance;

        [Header("Host Join")]

        [SerializeField] InputField joinMatchInput;
        [SerializeField] Button joinButton;
        [SerializeField] Button hostButton;

        [SerializeField] Canvas lobbyCanvas;

        [Header("Lobby")]
        [SerializeField] Transform UIPlayerParent;
        [SerializeField] GameObject UIPlayerPrefab;

        [SerializeField] Text matchIDText;
        [SerializeField] GameObject startGameButton;

        public void Host()
        {
            joinMatchInput.interactable = false;
            joinButton.interactable = false;
            hostButton.interactable = false;
            Debug.Log($"Player {Player.localPlayer}");
            Player.localPlayer.HostGame();

        }
        public void Join()
        {
            joinMatchInput.interactable = false;
            joinButton.interactable = false;
            hostButton.interactable = false;
            Player.localPlayer.JoinGame(joinMatchInput.text);
        }
        public void JoinSuccess(bool success, string matchID)
        {
            if(!success)
            {
            joinMatchInput.interactable = true;
            joinButton.interactable = true;
            hostButton.interactable = true;
            } else {
                lobbyCanvas.enabled = true;
                SpawnPlayerUIPrefab(Player.localPlayer);
                matchIDText.text = matchID;
            }

        }

        public void HostSuccess(bool success, string matchID){
            if(!success){
                joinMatchInput.interactable = true;
                joinButton.interactable = true;
                hostButton.interactable = true;
            } else {
                lobbyCanvas.enabled = true;
                SpawnPlayerUIPrefab(Player.localPlayer);
                matchIDText.text = matchID;
                startGameButton.SetActive(true);
            }
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