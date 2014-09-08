using UnityEngine;
using System.Collections;

public class ToggleSound : MonoBehaviour {
    UIToggle toggleSoundButton;

	// Use this for initialization
	void Awake () {
        toggleSoundButton = GetComponent<UIToggle>();
        toggleSoundButton.value = AudioListener.volume > 0;
        EventDelegate.Add(toggleSoundButton.onChange, OnChange);
	}

    void OnChange()
    {
        AudioListener.volume = toggleSoundButton.value ? 1 : 0;
        print("Toggle sound value " + toggleSoundButton.value.ToString());
        
    }
}
