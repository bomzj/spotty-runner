using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Classes.Utils;
using Assets.Scripts.Score.Facebook;
using Assets.Scripts.Score;
using GooglePlayGames;
using Assets.Scripts.Consts;

public class MainMenuController : MonoBehaviour 
{
    private UIButton leaderboardButton;
    private UIToggle toggleSoundButton;
    private UILabel welcomeLabel;

	// Use this for initialization
	void Start () {
        leaderboardButton = GameObject.Find("Leaderboard Button").GetComponent<UIButton>();
        toggleSoundButton = GameObject.Find("Toggle Sound Button").GetComponent<UIToggle>();
        toggleSoundButton.value = AudioListener.volume > 0;
        welcomeLabel = GameObject.Find("Welcome Title").GetComponent<UILabel>();
        UpdateUI();
	}

    void UpdateUI()
    {
        leaderboardButton.isEnabled = Social.localUser.authenticated;
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
        ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(GameConsts.GeneralLeaderboardID);
    }

    public void ShowAchievements()
    {
        Social.ShowAchievementsUI();
    }

    public void ToggleSound()
    {
        AudioListener.volume = toggleSoundButton.value ? 1 : 0;
        print("toggle sound " + toggleSoundButton.value.ToString());
    }

}
