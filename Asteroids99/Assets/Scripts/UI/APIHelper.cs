using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;

public static class APIHelper
{
    public static LeaderboardEntries GetLeaderboard()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://rest-api-asteroids99-dev.eu-central-1.elasticbeanstalk.com/api/scores");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        Debug.Log(json);
        var myObject = JsonUtility.FromJson<LeaderboardEntries>("{\"entries\":" + json + "}");
        //sortieren nach Score
        return myObject;
    }
}
