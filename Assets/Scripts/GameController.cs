using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.Scripts.Classes.Core;
using System;
using Assets.Scripts.Classes.Utils;
using Assets.Scripts.Ads;
using Assets.Scripts.Consts;
using GooglePlayGames;
using Assets.Scripts.Core;

/// <summary>
/// Gameplay state/controller/screen
/// </summary>
public class GameController : MonoBehaviour 
{
    private Pause pauseScript;

    public int GamesCountPlayed { get; private set;}
    private bool IsFirstTimePlayed { get { return GamesCountPlayed == 1; } }
        
    public enum GamePlayState
    {
        None,

        /// <summary>
        /// Game is in paused state and help panel is shown
        /// </summary>
        Help,
        
        /// <summary>
        /// Collectibles spawn is only paused, Giraffe animation is playing as well as other
        /// </summary>
        Countdown,
        
        /// <summary>
        /// Game is in running state: collectibles are being spawned , giraffe runs and etc
        /// </summary>
        Run,

        /// <summary>
        /// Game is in paused state: pause menu panel is shown
        /// </summary>
        Pause,

        /// <summary>
        /// Game is in paused state:
        /// </summary>
        GameOver
    }

    public GamePlayState gamePlayState {get; private set;}

    public event EventHandler GamePlayStateChanged;

    public AudioClip backgroundMusic;

	void Start () 
    {
        // Pause script
        pauseScript = GetComponent<Pause>();
        
        // update games count played 
        GamesCountPlayed = PlayerPrefs.GetInt(GameConsts.Settings.TimesReplayed, 0);
        PlayerPrefs.SetInt(GameConsts.Settings.TimesReplayed, ++GamesCountPlayed);

        // Show help panel if it is played for first time
        if (IsFirstTimePlayed)
        {
            SetGamePlayState(GamePlayState.Help);
        }
        else // show 3,2,1
        {
            SetGamePlayState(GamePlayState.Countdown);
        }

        if (!AudioManager.Instance.IsMusicPlaying(backgroundMusic))
        {
            AudioManager.Instance.PlayMusic(backgroundMusic, 0.3f);
        }
	}

    private void ShowHelpPanel()
    {
        var panels = Resources.FindObjectsOfTypeAll<UIPanel>();
        var helpPanels = panels.Where(item => item.name == "Help Panel");
        var helpPanel = helpPanels.First();
        NGUITools.SetActive(helpPanel.gameObject, true);
    }

    #region Game States

    public void SetGamePlayState(GamePlayState state)
    {
        print("Previous game play state is " + gamePlayState.ToString());
        switch (state)
        {
            case GamePlayState.Help:
                HandleHelpState();
                break;

            case GamePlayState.Countdown:
                HandleCountdownState();
                break;

            case GamePlayState.Run:
                HandleRunState();
                break;

            case GamePlayState.Pause:
                HandlePauseState();
                break;

            case GamePlayState.GameOver:
                HandleGameOverState();
                break;
        }

        this.gamePlayState = state;
        OnGamePlayStateChanged();
        print("New game play state is " + state.ToString());
    }

    private void OnGamePlayStateChanged()
    {
        if (GamePlayStateChanged != null)
        {
            GamePlayStateChanged(this, EventArgs.Empty);
        }
    }

    private void HandleHelpState()
    {
        HandlePauseState();
        ShowHelpPanel();
    }

    private IEnumerator ShowCountdownTimer()
    {
        // Skip one frame to sync with Pause functionality
        yield return null;

        print("Countdown timer started");
        // Set game state to countdown state
        var countdownTimerLabel = Resources.FindObjectsOfTypeAll<UILabel>()
            .Where(item => item.gameObject.name == "Countdown Timer Label").First();

        NGUITools.SetActive(countdownTimerLabel.gameObject, true);

        var tweens = countdownTimerLabel.GetComponents<UITweener>();

        for (int i = 0; i < 3; i++)
        {
            // Update text with new number
            countdownTimerLabel.text = (3 - i).ToString();

            // Play tweens (alpha and scale)
            foreach (var item in tweens)
            {
                item.ResetToBeginning();
                item.PlayForward();
            }

            yield return new WaitForSeconds(1);
        }

        NGUITools.SetActive(countdownTimerLabel.gameObject, false);

        SetGamePlayState(GamePlayState.Run);
    }

    private void HandleCountdownState()
    {
        var keepPaused = new string[] {"Collectibles", "Background", "Foreground", "Ground"};
        
        Pause[] pausableObjects = GameObject.FindObjectsOfType<Pause>();
        foreach (var item in pausableObjects)
        {
            // Pause movable objects sucha as collectibles and trees
            if (keepPaused.Contains(item.gameObject.name))
            {
                item.SetPause();
            }
            else // Unpause others
            {
                item.Resume();
            }
        }

        // Hide pause button 
        var pauseButton = GameObjectFinder.Find<UIButton>("Pause Button", true);
        pauseButton.SetActive(false);

        // Fade scene to clear
        var fadeScene = GetComponent<FadeScene>();
        fadeScene.FadeToClear();

        // Show "3,2,1" countdown timer
        pauseScript.StartPausableCoroutine(ShowCountdownTimer());
    }

    private void HandleRunState()
    {
        // Unpause all objects
        Pause[] pausableObjects = GameObject.FindObjectsOfType<Pause>();
        foreach (var item in pausableObjects)
        {
            if (item.isPaused)
            {
                item.Resume();
            }
        }

        if (gamePlayState == GamePlayState.Pause)
        { 
            // Fade scene to clear
            var fadeScene = GetComponent<FadeScene>();
            fadeScene.FadeToClear();

            var test1 = AudioManager.Instance.IsMusicPlaying(backgroundMusic);

            // resume sounds and music
            AudioManager.Instance.ResumeAllSounds();

            var test2 = AudioManager.Instance.IsMusicPlaying(backgroundMusic);

            // Start music if it was disabled previously
            if (!AudioManager.Instance.IsMusicPlaying(backgroundMusic))
            {
                AudioManager.Instance.PlayMusic(backgroundMusic);
            }
        }
        else
        {
            // Increase music volume 
            AudioManager.Instance.SetVolume(backgroundMusic, 1);
        }

        // show gui buttons
        var pauseButton = GameObjectFinder.Find<UIButton>("Pause Button", true);
        pauseButton.SetActive(true);

        // Spawn collectibles
    }

    private void HandlePauseState()
    {
        // Find pausable objects and pause them all
        Pause[] pausableObjects = GameObject.FindObjectsOfType<Pause>();
        foreach (var item in pausableObjects)
        {
            item.SetPause();
        }

        // Show pause menu
        // ShowPauseMenuPanel();

        // Hide pause button 
        var pauseButton = GameObjectFinder.Find<UIButton>("Pause Button", true);
        pauseButton.SetActive(false);

        // Fade scene to back
        var fadeScene = GetComponent<FadeScene>();
        fadeScene.FadeToBlack();

        // Pause music
        AudioManager.Instance.PauseAllSounds();

        print("Game paused");
    }

    private void HandleGameOverState()
    {
        // Hide pause button 
        var pauseButton = GameObjectFinder.Find<UIButton>("Pause Button", true);
        pauseButton.SetActive(false);

        StartCoroutine(StartGameOverState());
    }

    IEnumerator StartGameOverState()
    {
        // Stop music
        AudioManager.Instance.StopSound(backgroundMusic);

        // Hide collectibles 
        GameObject.Find("Collectibles").SetActive(false);
        
        // Find pausable objects and pause them all
        Pause[] pausableObjects = GameObject.FindObjectsOfType<Pause>();
        foreach (var item in pausableObjects)
        {
            item.SetPause();
        }

        yield return new WaitForSeconds(1);

        // show Game Over message
        var gameOverLabelObject = GameObjectFinder.Find("Game Over Label", true);
        gameOverLabelObject.SetActive(true);
        yield return new WaitForSeconds(3);
        gameOverLabelObject.SetActive(false);

        // Compare current score result with saved
        var scoreBarObject = GameObject.Find("Score Bar");
        ScoreBar scoreBar = scoreBarObject.GetComponent<ScoreBar>();
        var currentPlayerScore = scoreBar.CurrentScore;

        int bestPlayerScore = PlayerPrefs.GetInt(GameConsts.Settings.BestPlayerLocalScore, 0);
        
        if (currentPlayerScore > bestPlayerScore)
        {
            // Save global score and show leaderboard if player is playing first time
            if (Social.localUser.authenticated)
            {
                Social.ReportScore(currentPlayerScore, GameConsts.TheBestGiraffeLeaderboardID, result =>
                {
                    if (IsFirstTimePlayed)
                    {
                        ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(GameConsts.TheBestGiraffeLeaderboardID);
                    }    
                });
            }

            // Save local score
            PlayerPrefs.SetInt(GameConsts.Settings.BestPlayerLocalScore, currentPlayerScore);
        }
        
        ShowAdOrRestartGame();
    }

    void ad_Closed(object sender, EventArgs e)
    {
        var ad = sender as IAd;
        if (ad != null)
        {
            // remove event handler
            ad.Closed -= ad_Closed;
        }

        print("ad_Closed");

        // Send message to restart game or restart game
        RestartGame();
    }

    public void RestartGame()
    {
        Application.LoadLevel("GamePlay");
    }

    private void ShowAdOrRestartGame()
    {
        var random = new System.Random(DateTime.Now.Millisecond);
        // Show ad in 33% of all cases
        var showAdProbability = random.Next(3);
        if (showAdProbability == 0)
        {
            var adsManager = ServiceLocator.GetService<AdsManager>();
            var ad = adsManager.ShowInterstitialAd();
            ad.Closed += ad_Closed;
        }
        else
        {
            RestartGame();
        }
    }

    public void SetCountdownState()
    {
        SetGamePlayState(GamePlayState.Countdown);
    }

    public void PauseGame()
    {
        SetGamePlayState(GamePlayState.Pause);
    }

    public void ResumeGame()
    {
        SetGamePlayState(GamePlayState.Run);
    }

    public void GameOver()
    {
        SetGamePlayState(GamePlayState.GameOver);
    }

    #endregion 
      
}
