using UnityEngine;
using System.Collections;
using Assets.Scripts.Core;

public class ToggleSound : MonoBehaviour {
    UIToggle toggleSoundButton;

    private bool soundUIToggleActivated;

	// Use this for initialization
	void Awake () {
        toggleSoundButton = GetComponent<UIToggle>();
        toggleSoundButton.value = AudioManager.Instance.SoundEnabled;
        EventDelegate.Add(toggleSoundButton.onChange, OnChange);
	}

    void OnChange()
    {
        // hack : ngui UIToggle.onChange is called at startup, so if it is the first we just ignore sound toggle
        if (soundUIToggleActivated)
        {
            AudioManager.Instance.ToggleSoundOnOff();
            //toggleSoundButton.value = AudioManager.Instance.SoundEnabled;
        }
        soundUIToggleActivated = true;
    }
}
