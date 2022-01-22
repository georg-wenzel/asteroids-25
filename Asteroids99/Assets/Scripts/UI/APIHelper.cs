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

    private IEnumerator CallAPI(string url, Action<string> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
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
}
