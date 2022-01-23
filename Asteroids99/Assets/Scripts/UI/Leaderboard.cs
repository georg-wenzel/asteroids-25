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
                if(max_count == 11){
                    break;
                }
                GameObject item = Instantiate(LeaderboardItemPrefab, LeaderboardHolder);
                item.GetComponent<LeaderboardItem>().placement.text = entries.entries[i].placement.ToString();
                item.GetComponent<LeaderboardItem>().username.text = entries.entries[i].nickname;
                item.GetComponent<LeaderboardItem>().score.text = entries.entries[i].score.ToString();
                item.GetComponent<LeaderboardItem>().hits.text = entries.entries[i].destroyed_enemies.ToString();
                max_count++;
            }
        } else {
            GameObject item = Instantiate(LeaderboardItemPrefab, LeaderboardHolder);
            item.GetComponent<LeaderboardItem>().placement.text = "Server ";
            item.GetComponent<LeaderboardItem>().username.text = "request ";
            item.GetComponent<LeaderboardItem>().score.text = "failed!";
            item.GetComponent<LeaderboardItem>().hits.text = "";
        }
    }

    public void OnScorePosted(string data)
    {
        Debug.Log(data);
    }
    public void PostScore()
    {
        this.LogLog("Posting Score to server");
        APIHelper _helper = new APIHelper();
        string data = @"{" + @"""nickname"": ""Fabian""," + @"""score"": 1397," + @"""destroyed_enemies"": 19" + @"}";        
        Debug.Log(data);
        StartCoroutine(_helper.PostScore(OnScorePosted, data));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
