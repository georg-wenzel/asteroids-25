using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Networking
{
    public class NetworkHelper : MonoBehaviour
    {

        public void LoadScene(string sceneName)
        {
            //Start loading the Scene asynchronously and output the progress bar
            StartCoroutine(LoadSceneEnumerator(sceneName));
        }

        public IEnumerator LoadSceneEnumerator(string sceneName)
        {
            yield return null;

            //Begin to load the Scene you specify
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            //Don't let the Scene activate until you allow it to
            asyncOperation.allowSceneActivation = false;
            this.LogLog("Pro :" + asyncOperation.progress);
            while (asyncOperation.progress < 0.9f)
            {
                this.LogLog("Loading scene " + " [][] Progress: " + asyncOperation.progress);
                yield return null;
            }
            asyncOperation.allowSceneActivation = true;
            while (!asyncOperation.isDone)
            {
                this.LogLog("Scence is not Done");
                yield return null;
            }
            Scene nThisScene = SceneManager.GetSceneByName(sceneName);

            if (nThisScene.IsValid())
            {
                this.LogLog("Scene is Valid");
                SceneManager.SetActiveScene(nThisScene);
            }

        }

    }

}