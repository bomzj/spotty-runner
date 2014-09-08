using UnityEngine;
using System.Collections;

public class IntroController : MonoBehaviour {

    public float delayBeforeFadeOut = 2f;
    FadeScene sceneFadeInOut;

	// Use this for initialization
	void Start () {
        sceneFadeInOut = GameObject.FindObjectOfType<FadeScene>();
        sceneFadeInOut.FadeToClearCompleted += sceneFadeInOut_FadeToClearCompleted;
        sceneFadeInOut.FadeToBlackCompleted += sceneFadeInOut_FadeToBlackCompleted;
	}

    void sceneFadeInOut_FadeToClearCompleted()
    {
        StartCoroutine(FadeSceneToBlack());
    }

    void sceneFadeInOut_FadeToBlackCompleted()
    {
        LoadMainMenu();
    }

    IEnumerator FadeSceneToBlack()
    {
        yield return new WaitForSeconds(delayBeforeFadeOut);
        sceneFadeInOut.FadeToBlack();
    }

    void LoadMainMenu()
    {
        Debug.Log("LoadMainMenu");
        Application.LoadLevel("MainMenu");
    }
}
