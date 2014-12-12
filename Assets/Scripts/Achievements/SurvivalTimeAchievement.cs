using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Achievements
{
    public class SurvivalTimeAchievement : MonoBehaviour
    {
        private const string SurvivalTimeX30AchievementID = "CgkIkYy7-sYWEAIQBQ";
        private const string SurvivalTimeX60AchievementID = "CgkIkYy7-sYWEAIQBg";

        private bool isSurvivalTimeX30Unlocked;
        private bool isSurvivalTimeX60Unlocked;

        private float TimePlayed
        {
            get
            {
                // 3 is subtracted because of game count down timer (3,2,1)
                return Time.timeSinceLevelLoad - 3;
            }
        }

        public void Start()
        {
            isSurvivalTimeX30Unlocked = AchievementManager.Instance.IsAchievementUnlocked(SurvivalTimeX30AchievementID);
            isSurvivalTimeX60Unlocked = AchievementManager.Instance.IsAchievementUnlocked(SurvivalTimeX60AchievementID);
        }

        public void Update()
        {
            if (!isSurvivalTimeX60Unlocked && TimePlayed >= 60)
            {
                AchievementManager.Instance.UnlockAchievement(SurvivalTimeX60AchievementID);
                isSurvivalTimeX60Unlocked = true;
            }
            else if (!isSurvivalTimeX30Unlocked && TimePlayed >= 30)
            {
                AchievementManager.Instance.UnlockAchievement(SurvivalTimeX30AchievementID);
                isSurvivalTimeX30Unlocked = true;
            }
           
        }
    }
}
