using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.SocialPlatforms;

namespace Assets.Scripts.Score.Facebook
{
    class FacebookSocialPlatform : ISocialPlatform, IAuthenticationProvider
    {
        public FacebookSocialPlatform()
        {
            // create default user
            localUser = new FacebookLocalUser();
        }

        public void Authenticate(ILocalUser user, Action<bool> callback)
        {
            throw new NotImplementedException();
        }

        public IAchievement CreateAchievement()
        {
            throw new NotImplementedException();
        }

        public ILeaderboard CreateLeaderboard()
        {
            throw new NotImplementedException();
        }

        public bool GetLoading(ILeaderboard board)
        {
            throw new NotImplementedException();
        }

        public void LoadAchievementDescriptions(Action<IAchievementDescription[]> callback)
        {
            throw new NotImplementedException();
        }

        public void LoadAchievements(Action<IAchievement[]> callback)
        {
            throw new NotImplementedException();
        }

        public void LoadFriends(ILocalUser user, Action<bool> callback)
        {
            throw new NotImplementedException();
        }

        public void LoadScores(ILeaderboard board, Action<bool> callback)
        {
            throw new NotImplementedException();
        }

        public void LoadScores(string leaderboardID, Action<IScore[]> callback)
        {
            throw new NotImplementedException();
        }

        public void LoadUsers(string[] userIDs, Action<IUserProfile[]> callback)
        {
            throw new NotImplementedException();
        }

        public void ReportProgress(string achievementID, double progress, Action<bool> callback)
        {
            throw new NotImplementedException();
        }

        public void ReportScore(long score, string board, Action<bool> callback)
        {
            throw new NotImplementedException();
        }

        public void ShowAchievementsUI()
        {
            throw new NotImplementedException();
        }

        public void ShowLeaderboardUI()
        {
            throw new NotImplementedException();
        }

        public ILocalUser localUser
        {
            get;
            private set;
        }


        public void Logout()
        {
            localUser = new FacebookLocalUser();
        }
    }
}
