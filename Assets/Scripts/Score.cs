using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

    // Score
    private int scoreCount;
    private GameObject scoreCountGameObject;
    private PulseTextTween scoreCountPulseTween;

	// Use this for initialization
	void Start () {
        scoreCountGameObject = GameObject.Find("GUI/Score/Count");
        scoreCountPulseTween = scoreCountGameObject.GetComponent<PulseTextTween>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    #region Score

    public void AddScore(int amount)
    {
        scoreCount += amount;
        UpdateCountDisplay();
    }

    private void UpdateCountDisplay()
    {
        scoreCountGameObject.guiText.text = scoreCount.ToString();

        if (scoreCountPulseTween.IsTweenCompleted)
        {
            scoreCountPulseTween.PulseGUIText();
        }
    }

    #endregion 
}
