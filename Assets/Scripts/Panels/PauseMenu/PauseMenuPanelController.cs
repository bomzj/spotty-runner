using UnityEngine;
using System.Collections;

public class PauseMenuPanelController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void NavigateToMainMenu()
    {
        //Application.LoadLevel("MainMenu");
        Application.LoadLevel("GamePlay");
    }
}
