using UnityEngine;
using System.Collections;

public class CountdownTimer : MonoBehaviour {

    private GameObject textGameObject;
    private bool wasTween;
    
    // How much to scale text for pulse
    private float scaleAmount;

    private GUIText guiText;
    private int initialFontSize;

	// Use this for initialization
	void Start () {
        textGameObject = new GameObject();
        this.guiText = textGameObject.AddComponent<GUIText>();
        this.guiText.text = "3";
        this.guiText.color = Color.black;
        this.guiText.fontSize = 32;
        this.textGameObject.transform.Translate(new Vector3(0.5f, 0.2f));
        this.guiText.anchor = TextAnchor.MiddleCenter;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        //Debug.Log(string.Format("Resolution is {0}x{1}", Screen.width, Screen.height));
        GUI.color = Color.black;
        //GUI.Label(new Rect(10, 10, 100, 100), "3");
        if (Time.time > 3 && !wasTween)
        {
            wasTween = true;
        }
        
    }

   
}
