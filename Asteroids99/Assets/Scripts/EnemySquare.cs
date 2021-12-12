using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using Networking;
using Mirror;

[System.Serializable]
public class EnemySquare : MonoBehaviour
{

    #region fields

    [SerializeField] GameObject gameOverUI; // red X, active when player is dead
    [SerializeField] TMP_Text playerName;
    [SerializeField] TMP_Text playerScore;
    [SerializeField] TMP_Text playerHP;
    [SerializeField] GameObject enemyBoxes;

    public Player localPlayer;

    public GameState currentGameState {get;private set;}

    #endregion

    #region methods

    void Start()
    {
        gameOverUI.SetActive(false);
    }

    // [TargetRpc]
    // public void SetLocalPlayerOnClient(Player player)
    // {
    //     this.localPlayer = player;
    // }

    // [Command]
    // public void SetLocalPlayer(Player player)
    // {
    //     SetLocalPlayerOnClient(player);
    // }

    public void UpdateUI(GameState gameState)
    {
        // playerName.text = "Player_" + localPlayer.playerIndex.ToString(); // TODO: delete later
        playerHP.SetText(gameState.HP.ToString());
        playerScore.SetText(gameState.Score.ToString());
        if(gameState.GameOver)
        {
            gameOverUI.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag.Equals("TODO")) // TODO
        {
            EnemyBoxes eb = (EnemyBoxes)enemyBoxes.GetComponent(typeof(EnemyBoxes));
            eb.SpawnAsteroidOnOtherPlayer(this);
        }
    }

    #endregion
}
