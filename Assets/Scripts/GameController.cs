using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.Scripts.Classes.Models;
using Assets.Scripts.Classes.Core;
using System;
using Assets.Scripts.Classes.Utils;
using Assets.Scripts.Ads;
using Assets.Scripts.Consts;

/// <summary>
/// Gameplay state/controller/screen
/// </summary>
public class GameController : MonoBehaviour 
{
    // Object Pool
    private ObjectPool objectPool;
    public ObjectPool ObjectPool { get { return objectPool; } }

    // Message bus
    public MessageBus MessageBus { get; private set; }

    private Pause pauseScript;
        
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

    public GameObject leaderboardPanel;

	// Use this for initialization
	void Start () 
    {
        // Message bus
        MessageBus = new MessageBus();
        
        // Pool
        objectPool = new ObjectPool();

        //HideAllPanels();
        
        // Pause script
        pauseScript = GetComponent<Pause>();
        
        // Show help panel if it is played for first time
        var firstTimePlay = PlayerPrefs.GetInt(GameConsts.Settings.FirstTimePlay, 1);
        if (firstTimePlay != 0)
        {
            SetGamePlayState(GamePlayState.Help);
            
            // Set setting that game is already played
            PlayerPrefs.SetInt(GameConsts.Settings.FirstTimePlay, 0);
        }
        else
        {
            SetGamePlayState(GamePlayState.Countdown);
        }
       
	}
	
	// Update is called once per frame
	void Update () 
    {
	  
	}

    void OnLevelWasLoaded(int level)
    {
        //if (level == 13)
        print("Woohoo OnLevelWasLoaded");

    }

    #region Object Pool

    public void RecycleGameObject(GameObject gameObject)
    {
        string tag = gameObject.tag ?? gameObject.name;
        //int objectsInPoolCount = ObjectPool.GetObjectsCountByTag(tag);
        //if (objectsInPoolCount > objectsInPoolByTagMaxCount)
        //{
        //    Destroy(gameObject);
        //}
        //else
        //{
        //    ObjectPool.AddObjectToPool(tag, gameObject);
        //}
    }

    public void GetGameObjectFromPoolOrCreate(string tag)
    {
        //var obj = ObjectPool.GetObjectFromPool(tag);
        //if (obj == null)
        //{
        //    switch (tag)
        //    {
        //        case GameObjectTagNames.Apple:
        //            break;

        //        case GameObjectTagNames.Bomb:
        //            break;

        //    }
        //}
    }

    #endregion

    #region Collectibles

    public IEnumerator SpawnCollectibles()
    {
        yield return null;
    }
       

    #endregion

    #region GUI Panels

    // Is not used 
    private void ShowPauseMenuPanel()
    {
        var pauseMenuPanel = GameObject.Find("Pause Menu Panel");
        NGUITools.SetActive(pauseMenuPanel, true);
    }

    private void ShowHelpPanel()
    {
        var panels = Resources.FindObjectsOfTypeAll<UIPanel>();
        var helpPanels = panels.Where(item => item.name == "Help Panel");
        var helpPanel = helpPanels.First();
        NGUITools.SetActive(helpPanel.gameObject, true);
        //var tweens = helpPanel.GetComponents<UITweener>();
        //tweens.
    }

    private void ShowLeaderboardPanel()
    {
        //var leaderboardPanelObject = GameObjectFinder.Find("Leaderboard Panel", true);
        leaderboardPanel.SetActive(true);
    }

    private void ShowPanel()
    {
        
    }

    private void HidePanel()
    {

    }

    private void HideAllPanels()
    {
        var uiRoot = GameObject.Find("UI Root");
        // Take all panels except ui root panel
        var panels = uiRoot.GetComponentsInChildren<UIPanel>().Skip(1);
        
        foreach (var item in panels)
        {
            NGUITools.SetActive(item.gameObject, false);
        }
    }

    private void ShowHideButtons(bool show)
    {
        var buttons = GameObjectFinder.FindObjectsOfType<UIButton>(true);

        foreach (var item in buttons)
        {
            NGUITools.SetActive(item.gameObject, show);
        }
    }

    #endregion

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

        yield return new WaitForSeconds(1);

        // Pause movement objects such as trees
        //HandlePauseState();

        // Compare current score result with saved
        var scoreBarObject = GameObject.Find("Score Bar");
        ScoreBar scoreBar = scoreBarObject.GetComponent<ScoreBar>();
        var currentPlayerScore = scoreBar.CurrentScore;

        int bestPlayerScore = PlayerPrefs.GetInt(GameConsts.Settings.BestPlayerLocalScore, 0);

        if (currentPlayerScore > bestPlayerScore)
        {
            // Save global score and show leaderboard
            if (Social.localUser.authenticated)
            {
                Social.ReportScore(currentPlayerScore, GameConsts.GeneralLeaderboardID, PlayerScoreReportedHandler);
            }
            else // Save local score
            {
                PlayerPrefs.SetInt(GameConsts.Settings.BestPlayerLocalScore, currentPlayerScore);
            }
        }
        // Show ads
        else
        {
            ShowAdOrRestartGame();
        }
    }

    void PlayerScoreReportedHandler(bool result)
    {
        if (result)
        {
            print("Score reported");
            var scoreBar = GameObject.Find("Score Bar").GetComponent<ScoreBar>();
            PlayerPrefs.SetInt(GameConsts.Settings.BestPlayerGlobalScore, scoreBar.CurrentScore);
            
            // Get local user score in leaderboard
            var leaderboard = Social.CreateLeaderboard();
            leaderboard.id = GameConsts.GeneralLeaderboardID;
            leaderboard.SetUserFilter(new[] { Social.localUser.id });
            leaderboard.LoadScores(r => 
            {
                // Show panel if user exists in leaderboard
                if (r)
                {
                    if (leaderboard.scores != null && leaderboard.scores.Count() > 0)
                    {
                        ShowLeaderboardPanel();
                    }
                }
                else
                {
                    print("Failed to load local user score");
                    // Just Restart Game in case of error
                    ShowAdOrRestartGame();
                }
            });
        }
        else
        {
            print("Failed to report score");
            ShowAdOrRestartGame();
        }
    }

    void ad_Closed(object sender, EventArgs e)
    {
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
