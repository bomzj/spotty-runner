using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Classes.Utils;
using Assets.Scripts.Score.Facebook;
using Assets.Scripts.Score;

public class MainMenuController : MonoBehaviour 
{
    private UIButton leaderboardButton;
    private UIToggle toggleSoundButton;
    private UI2DSprite loginButtonUI2DSprite;
    private UILabel loginLabel;
    private UILabel welcomeLabel;

    private bool logging;

    public Sprite loginButtonSprite;
    public Sprite logoutButtonSprite;
    public Transform leaderboardPanelPrefab;

	// Use this for initialization
	void Start () {
        leaderboardButton = GameObject.Find("Leaderboard Button").GetComponent<UIButton>();
        toggleSoundButton = GameObject.Find("Toggle Sound Button").GetComponent<UIToggle>();
        toggleSoundButton.value = AudioListener.volume > 0;
        loginButtonUI2DSprite = GameObject.Find("Facebook Login Button").GetComponent<UI2DSprite>();
        loginLabel = GameObject.Find("Facebook Login Title").GetComponent<UILabel>();
        welcomeLabel = GameObject.Find("Welcome Title").GetComponent<UILabel>();
        UpdateUI();
	}
	
	// Update is called once per frame
	void Update () {
           
	}

    void UpdateUI()
    {
        leaderboardButton.isEnabled = Social.localUser.authenticated;
        loginButtonUI2DSprite.sprite2D = !Social.localUser.authenticated ? loginButtonSprite : logoutButtonSprite;
        loginLabel.text = !Social.localUser.authenticated ? "Login" : "Logout";
        welcomeLabel.enabled = Social.localUser.authenticated;
        welcomeLabel.text = string.Format("Welcome, {0}!", Social.localUser.userName);
    }

    #region Callbacks

    public void AuthenticationResultCallback(bool authenticated)
    {
        UpdateUI();
        logging = false;
    }

    #endregion


    #region Button Handlers

    public void PlayGame()
    {
        Application.LoadLevel("GamePlay");
    }

    public void DisablePlayButton()
    {
        var playButton = GameObject.Find("Play Button");
        playButton.SetActive(false);
    }

    public void EnablePlayButton()
    {
        var playButton = GameObjectFinder.Find("Play Button", true);
        playButton.SetActive(true);
    }

    public void DisposeLeaderboard()
    {
         var leaderboardPanel = GameObjectFinder.Find("Leaderboard Panel", true);
         Destroy(leaderboardPanel);
    }

    public void ToggleLogin()
    {
        if (!Social.localUser.authenticated)
        {
            Login();
        }
        else
        {
            Logout();
        }
    }

    public void Login()
    {
        if (!logging)
        {
            Social.localUser.Authenticate(AuthenticationResultCallback);
        }
    }

    public void Logout()
    {
        (Social.Active as IAuthenticationProvider).Logout();
        UpdateUI();
    }

    public void ToggleSound()
    {
        AudioListener.volume = toggleSoundButton.value ? 1 : 0;
        print("toggle sound " + toggleSoundButton.value.ToString());
    }

    #endregion

}
