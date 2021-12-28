using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Leaderboard : MonoBehaviour
{
    #region fields

    [SerializeField] Transform LeaderboardHolder;

    [SerializeField] GameObject LeaderboardItemPrefab;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        LeaderboardEntries lb = APIHelper.GetLeaderboard();
        for (int i = 0; i < lb.entries.Length; i++)
        {
            //Nach 10 Einträgen abbrechen
            GameObject item = Instantiate(LeaderboardItemPrefab, LeaderboardHolder);
            item.GetComponent<LeaderboardItem>().username.text = lb.entries[i].nickname;
            item.GetComponent<LeaderboardItem>().placement.text = lb.entries[i].placement;
            item.GetComponent<LeaderboardItem>().score.text = lb.entries[i].score;
        }
        
    }

    public void ReturnToMainMenu()
    {
        // Load some Scene
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
