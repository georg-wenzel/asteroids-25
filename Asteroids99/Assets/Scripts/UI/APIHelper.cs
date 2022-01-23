using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using System.Linq;
using UnityEngine.Networking;

public class APIHelper
{
    private const string API_URL = "http://rest-api-asteroids99-dev.eu-central-1.elasticbeanstalk.com/api/scores";

    private IEnumerator CallAPI(string url, Action<string> callback, String data = null)
    {
        using (UnityWebRequest request = (data == null) ?
            UnityWebRequest.Get(url) : UnityWebRequest.Put(url, data))
        {
            if (request.method.Equals("PUT"))
            {
                request.SetRequestHeader ("Content-Type", "application/json");
                request.method = "POST";
            }
            yield return request.Send();

            if (request.isNetworkError)
            {
                Debug.LogError("Network Problem: " + request.error);
            }
            else if (request.responseCode != (long) System.Net.HttpStatusCode.OK)
            {
                Debug.LogError("Response Error: " + request.responseCode);
            }
            else
            {
                callback(request.downloadHandler.text);
            }
        }
    }


    public IEnumerator GetLeaderboard(Action<string> callback)
    {
        return CallAPI(API_URL, callback);
    }

    public IEnumerator PostScore(Action<string> callback, string data)
    {
        return CallAPI(API_URL, callback, data);
    }
}
