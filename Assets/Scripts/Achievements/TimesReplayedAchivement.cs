using Assets.Scripts.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Achievements
{
    public class TimesReplayedAchivement : MonoBehaviour
    {
        private const string TimesReplayedX100AchievementID = "CgkIkYy7-sYWEAIQBw";
        private const string TimesReplayedX500AchievementID = "CgkIkYy7-sYWEAIQCA";
        
        public void Start()
        {
            // All settings must be in SettingsManager
            int timesReplayedCount = PlayerPrefs.GetInt(GameConsts.Settings.TimesReplayed, 0);
            AchievementManager.Instance.IncrementAchievement(TimesReplayedX500AchievementID, 1);
            AchievementManager.Instance.IncrementAchievement(TimesReplayedX100AchievementID, 1);
        }
        
    }
}
