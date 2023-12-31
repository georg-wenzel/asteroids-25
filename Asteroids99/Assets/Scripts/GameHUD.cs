using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using Utils;
using System.Linq;
using Networking;

public class GameHUD : MonoBehaviour
{

    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject winningPanel;
    [SerializeField] List<Image> hp_images;
    [SerializeField] Sprite hp_0Life;
    [SerializeField] Sprite hp_1Life;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text PKsText;
    [SerializeField] TMP_Text PlacementText;


    public void UpdatePlacement(int amountActivePlayers)
    {
        PlacementText.text = amountActivePlayers.ToString();
    }

    public void UpdateHP(GameState gameState)
    {
        if(!HasWon())
        {
            int hp = gameState.HP;
            if (hp > hp_images.Capacity)
                Debug.Log("HP and amount of HP-symbols do not fit");
            for (int i=hp_images.Capacity; i>0; i--)
            {
                if (i > hp)
                {
                    hp_images[i-1].sprite = hp_0Life;
                }
                else
                    break;
            }
        }
    }

    public void UpdateScore(GameState gameState)
    {
        scoreText.text = gameState.Score.ToString();
    }

    public void UpdatePKs(GameState gameState)
    {
        PKsText.text = gameState.PlayerKills.ToString();
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public bool HasWon()
    {
        return winningPanel.activeSelf;
    }

    public void GameWon()
    {
        winningPanel.SetActive(true);
    }

    public void ExitGame()
    {
        //SceneManager.LoadScene("MainMenu");
        GameObject.Find("MultiplayerUI").GetComponent<MultiplayerUI>().goToMainMenu();
    }

    public void OnScorePosted(string data)
    {
        //Debug.Log(data);
        GameObject.Find("MultiplayerUI").GetComponent<MultiplayerUI>().goToMainMenu();
    }

    public void SaveScore()
    {
        this.LogLog("Saving Score to server");
        LeaderboardEntry entry = new LeaderboardEntry();
        entry.nickname = Player.localPlayer.playerName;
        entry.placement = int.Parse(PlacementText.text);
        entry.score = int.Parse(scoreText.text);
        entry.destroyed_enemies = int.Parse(PKsText.text);
        APIHelper _helper = new APIHelper();
        var data = JsonConvert.SerializeObject(entry, Formatting.Indented);
        Debug.Log(data);
        StartCoroutine(_helper.PostScore(OnScorePosted, data));
    }

}
