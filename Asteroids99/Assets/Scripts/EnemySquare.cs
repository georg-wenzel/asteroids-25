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
        // set playerName, starting Score and HP
    }

    void UpdateUIOnChanges()
    {
        // if (!player.isDead())
        // { 
        //     getPlayerData();
        //     updateUI();
        //     if (player.isDead())
        //         gameOverUI.SetActive(true);
        // }
    }

    #endregion
}
