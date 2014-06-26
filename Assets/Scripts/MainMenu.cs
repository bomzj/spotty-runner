using UnityEngine;
using System.Collections;
using System;

public class MainMenu : MonoBehaviour {

    public Sprite playButtonSprite;

    private GUIStyle buttonGUIStyle;
    private Color highlightColor = new Color(0.85f,0.85f,0.85f);
    private bool isClicked;

    public event Action action;

	// Use this for initialization
	void Start () {
        buttonGUIStyle = new GUIStyle();
        buttonGUIStyle.normal.background = playButtonSprite.texture;
        buttonGUIStyle.active.background = playButtonSprite.texture;
        buttonGUIStyle.hover.background = playButtonSprite.texture;
	}
	
	// Update is called once per frame
	void Update () {
           
	}

    void OnGUI()
    {
        // Make a background box
        //GUI.Box(new Rect(10, 10, 100, 90), "Loader Menu");
        
        if (isClicked)
        {
            // For highlight the button , should be called before gui.button call
            GUI.color = highlightColor;
        }
        else
        {
            //GUI.color = Color.white;
        }

        // Play button
        if (GUI.Button(new Rect(20, 40, 80, 20), "", buttonGUIStyle))
        {
            isClicked = true;
            Application.LoadLevel("GamePlay");
        }
    }
}
