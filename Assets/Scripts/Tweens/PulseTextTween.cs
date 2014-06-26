using UnityEngine;
using System.Collections;

public class PulseTextTween : MonoBehaviour {

    // How much to scale text for pulse
    public float scaleAmount;
    public float pulseTime;
    public iTween.EaseType easeType;

    private GUIText guiText;
    private int initialFontSize;

    [HideInInspector]
    public bool IsTweenCompleted = true;

    // Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    /// <summary>
    /// Pulses gui text of the game object
    /// </summary>
    /// <param name="guiText"></param>
    public void PulseGUIText()
    {
        PulseGUIText(this.gameObject.guiText);
    }

    private void PulseGUIText(GUIText guiText)
    {
        Debug.Log("PulseText is called with initial fontSize" + guiText.fontSize);

        IsTweenCompleted = false;

        this.guiText = guiText;
        this.initialFontSize = guiText.fontSize;

        Hashtable ht = iTween.Hash(
            "from", initialFontSize,
            "to", scaleAmount * initialFontSize,
            "onupdate", "OnPulseUpdate",
            "onupdatetarget", this.gameObject,
            "time", pulseTime / 2,
            "oncomplete", "OnHalfPulseComplete",
            "oncompletetarget", this.gameObject,
            "easetype", easeType);

        iTween.ValueTo(this.gameObject, ht);
    }

    private void OnPulseUpdate(int newFontSize)
    {
        this.guiText.fontSize = newFontSize;
    }

    private void OnHalfPulseComplete()
    {
        Hashtable ht = iTween.Hash(
            "from", scaleAmount * initialFontSize,
            "to", initialFontSize,
            "onupdate", "OnPulseUpdate",
            "onupdatetarget", this.gameObject,
            "time", pulseTime/2,
            "oncomplete", "OnPulseComplete",
            "oncompletetarget", this.gameObject,
            "easetype", easeType);

        iTween.ValueTo(this.gameObject, ht);

        Debug.Log("OnHalfPulseComplete fontSize is " + this.guiText.fontSize);
    }

    private void OnPulseComplete()
    {
        IsTweenCompleted = true;
        Debug.Log("OnPulseComplete fontSize is " + this.guiText.fontSize);
    }
}
