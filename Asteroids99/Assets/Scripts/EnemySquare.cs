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
    public enum FieldDirection
    {
        UP, DOWN, LEFT, RIGHT
    }

    [SerializeField] FieldDirection GameFieldDirection;
    [SerializeField] GameObject gameOverUI; // red X, active when player is dead
    [SerializeField] TMP_Text playerName;
    [SerializeField] TMP_Text playerScore;
    [SerializeField] TMP_Text playerHP;
    [SerializeField] GameObject enemyBoxes;

    public Player localPlayer;

    public int playerIndex;

    public bool isGameOver = false;

    public GameState currentGameState {get;private set;}

    public ParticleSystem particlesUp;
    public ParticleSystem particlesDown;
    public ParticleSystem particlesLeft;
    public ParticleSystem particlesRight;

    public AudioSource attackAudio;

    #endregion

    #region methods

    void Start()
    {
        gameOverUI.SetActive(false);
    }

    public void UpdateUI(GameState gameState, string name = "")
    {
        playerName.text = name;
        playerHP.SetText(gameState.HP.ToString());
        playerScore.SetText(gameState.Score.ToString());
        isGameOver = gameState.GameOver;
        if(gameState.GameOver)
        {
            gameOverUI.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(Player.localPlayer.gameState.GameOver)
        {
            if(other.gameObject.tag.Equals("FriendlyAsteroid"))
            {
                Destroy(other.gameObject);
                return;
            }
        }
        if(other.gameObject.tag.Equals("FriendlyAsteroid"))
        {
            EnemyBoxes eb = (EnemyBoxes)enemyBoxes.GetComponent(typeof(EnemyBoxes));
            eb.SpawnAsteroidOnOtherPlayer(this);
            Destroy(other.gameObject); // destroy friendlyAsteroid

            //Play particle effect
            switch(this.GameFieldDirection)
            {
                case FieldDirection.UP:
                    particlesUp.Play();
                    break;

                case FieldDirection.DOWN:
                    particlesDown.Play();
                    break;

                case FieldDirection.LEFT:
                    particlesLeft.Play();
                    break;

                case FieldDirection.RIGHT:
                    particlesRight.Play();
                    break;
            }

            //Play audio
            attackAudio.Play();
        }
    }

    #endregion
}
