using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System.Threading;
using Utils;

[System.Serializable]
public class Leaderboard : MonoBehaviour
{
    #region fields

    [SerializeField] Transform LeaderboardHolder;

    [SerializeField] GameObject LeaderboardItemPrefab;

    #endregion
    public LeaderboardEntries entries {get; private set;}
    // Start is called before the first frame update
    void Start()
    {
        this.LogLog("Getting Leaderboard from server");
        APIHelper _helper = new APIHelper();
        StartCoroutine(_helper.GetLeaderboard(OnAPIDataLoaded));

    }
    public void ReturnToMainMenu()
    {
        // Load some Scene
        SceneManager.LoadScene("MainMenu");
    }
    public void OnAPIDataLoaded(string data)
    {
        this.LogLog("Leaderboard data loaded");
        LeaderboardEntries _entries = null;
        _entries = JsonUtility.FromJson<LeaderboardEntries>("{\"entries\":" + data + "}");
            //sortieren nach Score
        if(entries == null){
            entries = new LeaderboardEntries();
        }    
        entries.entries = _entries.entries.OrderByDescending(c => c.score).ToArray();
        this.LogLog("set entries");
        if(entries != null){
            var max_count = 1;
            for (int i = 0; i < entries.entries.Length; i++)
            {
                //Nach 10 Einträgen abbrechen
                if(max_count == 10){
                    break;
                }
                GameObject item = Instantiate(LeaderboardItemPrefab, LeaderboardHolder);
                item.GetComponent<LeaderboardItem>().placement.text = entries.entries[i].placement;
                item.GetComponent<LeaderboardItem>().username.text = entries.entries[i].nickname;
                item.GetComponent<LeaderboardItem>().score.text = entries.entries[i].score;
                max_count++;
            }
        } else {
            GameObject item = Instantiate(LeaderboardItemPrefab, LeaderboardHolder);
            item.GetComponent<LeaderboardItem>().placement.text = "Server ";
            item.GetComponent<LeaderboardItem>().username.text = "request ";
            item.GetComponent<LeaderboardItem>().score.text = "failed!";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
