using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Networking;

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

    public void UpdateUI(GameState gameState)
    {
        currentGameState = gameState;
        if(!gameState.GameOver){
            playerHP.SetText(gameState.HP.ToString());
            playerScore.SetText(gameState.Score.ToString());
        }
        else if(gameState.GameOver)
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
