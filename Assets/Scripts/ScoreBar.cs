using UnityEngine;
using System.Collections;
using Assets.Scripts.Consts;

public class ScoreBar : MonoBehaviour {

    // TODO: Make as separate Score class
    public int CurrentScore { get; private set; }
    
    private UILabel currentScoreLabel;
    private UI2DSprite appleStub;
    
    // Pulse tween
    private UITweener pulseTween;
    bool isPulseTweenRunning;
    bool isPulseTweenUp;

	void Start () {
        var appleStubTransform = transform.Find("Apple Stub");
        appleStub = appleStubTransform.GetComponent<UI2DSprite>();
        
        var currentScore = transform.Find("Current Score");
        currentScoreLabel = currentScore.GetComponent<UILabel>();

        var bestScore = PlayerPrefs.GetInt(GameConsts.Settings.BestPlayerLocalScore, 0);
        var bestScoreLabel = transform.Find("Best Score").GetComponent<UILabel>();
        bestScoreLabel.text = string.Format("Best: {0}", bestScore);

        pulseTween = currentScore.GetComponent<TweenScale>();
        pulseTween.AddOnFinished(OnPulseUpDownFinished);
	}
	
    #region Score

    public void AddScore(int amount)
    {
        CurrentScore += amount;
        UpdateCountDisplay();
    }

    private void UpdateCountDisplay()
    {
        currentScoreLabel.text = CurrentScore.ToString();
        
        // Pulse if tween finished
        if (!isPulseTweenRunning)
        {
            Pulse();
        }
    }

    private void Pulse()
    {
        isPulseTweenRunning = true;
        isPulseTweenUp = true;
        pulseTween.Toggle();
    }

    private void OnPulseUpDownFinished()
    {
        if (isPulseTweenUp)
        {
            isPulseTweenUp = false;
            // start reverse
            pulseTween.Toggle();
        }
        else
        {
            isPulseTweenRunning = false;
        }
    }

    #endregion 
}
