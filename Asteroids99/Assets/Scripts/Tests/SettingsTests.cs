using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests
{
    public class SettingsTests
    {
        // Since current Tests are all navigation tests, we start from the main menu.
        [SetUp]
        public void LoadMainMenuScene()
        {
            SceneManager.LoadScene("MainMenu");
        }

        [UnityTest]
        public IEnumerator MainMenuToSettingsNavigationTest()
        {
            GameObject.Find("SettingsButton").GetComponent<Button>().onClick.Invoke();
            // after clicking setting there should be (among others) a button to set audio setting
            GameObject.Find("AudioButton").GetComponent<Button>();
            // Go Back to MainMenu and check if the settings button is there
            GameObject.Find("BackButton").GetComponent<Button>().onClick.Invoke();
            GameObject.Find("SettingsButton").GetComponent<Button>();
            // Also, AudioButton should no be visible any more
            Assert.IsNull(GameObject.Find("AudioButton")); 
            yield return null;
        }
    }
}
