using Assets.Scripts.Ads;
using Assets.Scripts.Ads.Fake;
using Assets.Scripts.Classes.Core;
using Assets.Scripts.Classes.Models;
using Assets.Scripts.Consts;
using Assets.Scripts.Score;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    /// <summary>
    /// Initialize game with custom settings, services and other needed stuff
    /// </summary>
    public class GameInitializer : MonoBehaviour
    {
        private static bool isInitialized;

        void Start()
        {
            if (!isInitialized)
            {
                InitializeGame();
            }
        }

        public void InitializeGame()
        {
            InitializeServiceLocator();
            InitializeGameSettings();
            
            isInitialized = true;
            Debug.Log("Game initialized");

        }

        private void InitializeServiceLocator()
        {
            ServiceLocator.AddService(new ObjectPool());
            ServiceLocator.AddService(new MessageBus());
            ServiceLocator.AddService(new ScoreManager());
            ServiceLocator.AddService(new AdsManager(new FakeAdsProvider()));
            Debug.Log("Game services initialized");
        }

        private void InitializeGameSettings()
        {
            // Mock Social Platform
            Social.Active = new FakeSocialPlatform();

             // For Debug : Make local user authenticated to show leaderboards 
            Social.localUser.Authenticate(result => { });
            // Reset score
            PlayerPrefs.SetInt(GameConsts.Settings.BestPlayerLocalScore, 0);
            
            //PlayerPrefs.SetInt(GameConsts.Settings.FirstTimePlay, 0);

           
        }
    }
}
