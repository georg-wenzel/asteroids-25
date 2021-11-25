using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Credits : MonoBehaviour
{

    public TMP_Text text;

    public GameObject mainmenuComponent;
    public GameObject creditsComponent;

    private bool showingCredits = false;

    void Update()
    {
        if(!showingCredits)
            StartCoroutine(showCredits());
        if(Input.GetKeyDown("escape") || Input.GetKeyDown("space"))
        {
            ReturnToMenu();
        }
    }

    IEnumerator showCredits()
    {
        showingCredits = true;
        StartCoroutine(showCreditsText("A Game\nMade in\nUniversity of Innsbruck"));
        yield return new WaitForSeconds(2.6f);
        StartCoroutine(showCreditsText("Bernhard Eder"));
        yield return new WaitForSeconds(2.6f);
        StartCoroutine(showCreditsText("Florian Maier"));
        yield return new WaitForSeconds(2.6f);
        StartCoroutine(showCreditsText("Fabian Rochelt"));
        yield return new WaitForSeconds(2.6f);
        StartCoroutine(showCreditsText("Georg Wenzel"));
        yield return new WaitForSeconds(2.6f);
        StartCoroutine(showCreditsText("Thanks for playing ASTEROIDS!"));
        yield return new WaitForSeconds(2.6f);
        ReturnToMenu();
    }

    IEnumerator showCreditsText(string s)
    {
        text.alpha = 1;
        text.text = s;
        yield return new WaitForSeconds(0.5f);
        text.alpha = 0.8f;
        yield return new WaitForSeconds(0.5f);
        text.alpha = 0.6f;
        yield return new WaitForSeconds(0.5f);
        text.alpha = 0.4f;
        yield return new WaitForSeconds(0.5f);
        text.alpha = 0.2f;
        yield return new WaitForSeconds(0.5f);
        text.alpha = 0;
    }

    void ReturnToMenu()
    {
        mainmenuComponent.SetActive(true);
        creditsComponent.SetActive(false);
        showingCredits = false;
    }
}
