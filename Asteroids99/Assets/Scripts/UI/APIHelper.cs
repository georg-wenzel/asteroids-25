using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using System.Linq;

public static class APIHelper
{
    public static LeaderboardEntries GetLeaderboard()
    {
        LeaderboardEntries myObject = null;
        try{
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://rest-api-asteroids99-dev.eu-central-1.elasticbeanstalk.com/api/scores");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();
            Debug.Log(json);
            myObject = JsonUtility.FromJson<LeaderboardEntries>("{\"entries\":" + json + "}");
            //sortieren nach Score
            myObject.entries = myObject.entries.OrderByDescending(c => c.score).ToArray();
        }
        catch(WebException e){
            Debug.Log("Server request failed: "+e.Message);
        }
        return myObject;
    }
}
