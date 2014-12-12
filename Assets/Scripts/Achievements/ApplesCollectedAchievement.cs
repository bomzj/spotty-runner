using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Achievements
{
    public class ApplesCollectedAchievement : MonoBehaviour
    {
        private const string ApplesCollectedX30AchievementID = "CgkIkYy7-sYWEAIQAg";
        private const string ApplesCollectedX50AchievementID = "CgkIkYy7-sYWEAIQAw";
        private const string ApplesCollectedX100AchievementID = "CgkIkYy7-sYWEAIQBA";

        private bool isApplesCollectedX30Unlocked;
        private bool isApplesCollectedX50Unlocked;
        private bool isApplesCollectedX100Unlocked;

        private ScoreBar scoreBar;

        public void Start()
        {
            var scoreBarObject = GameObject.Find("Score Bar");
            scoreBar = scoreBarObject.GetComponent<ScoreBar>();
            isApplesCollectedX30Unlocked = AchievementManager.Instance.IsAchievementUnlocked(ApplesCollectedX30AchievementID);
            isApplesCollectedX50Unlocked = AchievementManager.Instance.IsAchievementUnlocked(ApplesCollectedX50AchievementID);
            isApplesCollectedX100Unlocked = AchievementManager.Instance.IsAchievementUnlocked(ApplesCollectedX100AchievementID);
        }

        public void Update()
        {
            if (!isApplesCollectedX100Unlocked && scoreBar.CurrentScore >= 100)
            {
                AchievementManager.Instance.UnlockAchievement(ApplesCollectedX100AchievementID);
                isApplesCollectedX100Unlocked = true;
            }
            else if (!isApplesCollectedX50Unlocked && scoreBar.CurrentScore >= 50)
            {
                AchievementManager.Instance.UnlockAchievement(ApplesCollectedX50AchievementID);
                isApplesCollectedX50Unlocked = true;
            }
            else if (!isApplesCollectedX30Unlocked && scoreBar.CurrentScore >= 30)
            {
                AchievementManager.Instance.UnlockAchievement(ApplesCollectedX30AchievementID);
                isApplesCollectedX30Unlocked = true;
            }
        }
    }
}
