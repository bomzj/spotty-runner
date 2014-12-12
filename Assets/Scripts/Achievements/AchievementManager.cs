using GooglePlayGames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Assets.Scripts.Achievements
{
    public class AchievementManager
    {
        private List<IAchievement> achievements;

        private bool achievementsLoaded;
        private bool achievementsLoading;

        private static readonly AchievementManager instance = new AchievementManager();
        
        public static AchievementManager Instance
        {
            get
            {
                return instance;
            }
        }

        public void IncrementAchievement(string achievementID, int steps)
        {
            // increment achievement by 5 steps
            if (Social.localUser.authenticated)
            {
                ((PlayGamesPlatform)Social.Active).IncrementAchievement(achievementID, steps, (bool success) =>
                {
                    // handle success or failure
                });
            }
        }

        public void UnlockAchievement(string achievementID)
        {
            // check if user authenticated before any call
            if (Social.localUser.authenticated)
            {
                Social.ReportProgress(achievementID, 100.0f, (bool success) =>
                {
                    // handle success or failure
                });
            }
        }

        public bool IsAchievementUnlocked(string achievementID)
        {
            bool achivementUnlocked = false;

            if (achievementsLoaded && !achievementsLoading)
            {
                foreach (var item in achievements)
                {
                    if (item.id == achievementID)
                    {
                        achivementUnlocked = item.completed;
                    }
                }
            }

            return achivementUnlocked;
        }

        public void LoadAchievements()
        {
            Social.LoadAchievements(result =>
                {
                    achievements = result.ToList();
                    achievementsLoaded = true;
                    achievementsLoading = false;
                });
            achievementsLoading = true;
        }

       
    }
}
