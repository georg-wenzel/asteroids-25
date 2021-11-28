using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemySquare : MonoBehaviour
{

    #region fields

    public GameObject gameOverUI; // red X, active when player is dead
    public TMP_Text playerName;
    public TMP_Text playerScore;
    public TMP_Text playerHP;

    #endregion

    #region methods

    void Start()
    {
        gameOverUI.SetActive(false);
    }

    public void UpdateUI(GameState gameState)
    {
        if(!gameState.GameOver){
            playerHP.SetText(gameState.HP.ToString());
            playerScore.SetText(gameState.Score.ToString());
        }
        else if(gameState.GameOver)
        {
            gameOverUI.SetActive(true);
        }
    }

    #endregion
}
