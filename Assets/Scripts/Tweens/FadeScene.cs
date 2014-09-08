using UnityEngine;
using System.Collections;
using System;

public class FadeScene : MonoBehaviour {

    public float fadeTime;
    public float fadeAmount = 1;
    public iTween.EaseType easeType;

    public bool autoFadeIn = true;

    public event Action FadeToBlackCompleted;
    public event Action FadeToClearCompleted;

	// Use this for initialization
	void Awake () 
    {
        iTween.CameraFadeAdd();
	}
	
    void Start()
    {
        if (autoFadeIn)
        {
            FadeToClear();
        }
    }

    public void FadeToBlack()
    {
        Hashtable ht = iTween.Hash(
            "amount", fadeAmount,
            "time", fadeTime,
            "easetype", easeType,
            "oncomplete", "OnFadeToBlackCompleted",
            "oncompletetarget", this.gameObject);

        iTween.CameraFadeTo(ht);
    }

    public void FadeToClear()
    {
        Hashtable ht = iTween.Hash(
            "amount", -fadeAmount,
            "time", fadeTime,
            "easetype", easeType,
            "oncomplete", "OnFadeToClearCompleted",
            "oncompletetarget", this.gameObject);

        iTween.CameraFadeTo(ht);
    }

    private void OnFadeToBlackCompleted()
    {
        Debug.Log("OnFadeToBlackCompleted");
        if (FadeToBlackCompleted != null)
        {
            FadeToBlackCompleted();
        }
    }

    private void OnFadeToClearCompleted()
    {
        Debug.Log("OnFadeToClearCompleted");
        if (FadeToClearCompleted != null)
        {
            FadeToClearCompleted();
        }
    }
}
