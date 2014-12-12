using UnityEngine;
using GooglePlayGames;
using Assets.Scripts.Consts;
using Assets.Scripts.Core;

public class MainMenuController : MonoBehaviour 
{
    private UIButton leaderboardButton;
    private UIButton achievementsButton;
    private UIToggle toggleSoundButton;
    private UILabel welcomeLabel;

    public AudioClip backgroundMusic;

	// Use this for initialization
	void Start () {
        leaderboardButton = GameObject.Find("Leaderboard Button").GetComponent<UIButton>();
        achievementsButton = GameObject.Find("Achievements Button").GetComponent<UIButton>();
        //toggleSoundButton = GameObject.Find("Toggle Sound Button").GetComponent<UIToggle>();
        //toggleSoundButton.value = AudioManager.Instance.SoundEnabled;
        welcomeLabel = GameObject.Find("Welcome Title").GetComponent<UILabel>();
        UpdateUI();
        
        AudioManager.Instance.PlayMusic(backgroundMusic, 0.3f);
	}

    void UpdateUI()
    {
        leaderboardButton.isEnabled = achievementsButton.isEnabled = Social.localUser.authenticated;
        welcomeLabel.enabled = Social.localUser.authenticated;
        welcomeLabel.text = string.Format("Welcome, {0}!", Social.localUser.userName);
    }

    public void AuthenticationResultCallback(bool authenticated)
    {
        UpdateUI();
    }

    public void PlayGame()
    {
        // authenticate user:
        Social.localUser.Authenticate((bool success) =>
        {
            // handle success or failure
            Application.LoadLevel("GamePlay");
        });
    }

    public void ShowLeaderboard()
    {
        ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(GameConsts.TheBestGiraffeLeaderboardID);
    }

    public void ShowAchievements()
    {
        Social.ShowAchievementsUI();
    }

    //public void ToggleSound()
    //{
    //    // hack : ngui UIToggle is called at startup, so if it is the first we just ignore sound toggle
    //    if (soundUIToggleActivated)
    //    {
    //        AudioManager.Instance.ToggleSoundOnOff();
    //    }
    //    soundUIToggleActivated = true;
    //}

}
