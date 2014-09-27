using Assets.Scripts.Score.Fake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.SocialPlatforms;

namespace Assets.Scripts.Score
{
    class FakeSocialPlatform : ISocialPlatform, IAuthenticationProvider
    {
        FakeLeaderboard leaderboard;
        IEnumerable<FakeUserProfile> users;

        public FakeSocialPlatform()
        {
            leaderboard = GenerateFakeLeaderboard();
            users = GenerateFakeUsers();
            localUser = new FakeLocalUser();
        }

        public void Authenticate(ILocalUser user, Action<bool> callback)
        {
            
        }

        public IAchievement CreateAchievement()
        {
            throw new NotImplementedException();
        }

        public ILeaderboard CreateLeaderboard()
        {
            return leaderboard;
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
            callback(true);
        }

        public void LoadScores(string leaderboardID, Action<IScore[]> callback)
        {
           
            callback(leaderboard.scores);
        }

        public void LoadUsers(string[] userIDs, Action<IUserProfile[]> callback)
        {
            callback(users.ToArray());
        }

        public void ReportProgress(string achievementID, double progress, Action<bool> callback)
        {
            throw new NotImplementedException();
        }

        public void ReportScore(long score, string board, Action<bool> callback)
        {
            leaderboard = GenerateFakeLeaderboard();
            var scores = leaderboard.scores.ToList();
            var userScore = leaderboard.scores.FirstOrDefault(s => s.userID == localUser.id);
            // remove current user score
            scores.Remove(userScore);
            // Add new user score
            scores.Add(new FakeScore(FakeLeaderboard.LeaderboardID, localUser.id, (int)score));
            // recalculate ranks and order
            var newScores = leaderboard.CalculateRankAndOrder(scores);
            leaderboard.scores = newScores.ToArray();
            callback(true);
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

        private IEnumerable<FakeUserProfile> GenerateFakeUsers()
        {
            var users = new FakeUserProfile[]
            {
                new FakeUserProfile("fake1", "Fake1"),
                new FakeUserProfile("fake2", "Fake2"),
                new FakeUserProfile("fake3", "Fake3"),
                new FakeUserProfile("fake4", "Fake4"),
                new FakeUserProfile("fake5", "Fake5"),
                new FakeUserProfile("fake6", "Fake6"),
                new FakeUserProfile("fake7", "Fake7"),
                new FakeUserProfile("fake8", "Fake8"),
                new FakeUserProfile("fake9", "Fake9"),
                new FakeUserProfile(FakeLocalUser.UserID, FakeLocalUser.UserName),
            };

            return users;
        }

        private FakeLeaderboard GenerateFakeLeaderboard()
        {
            FakeLeaderboard leaderboard = new FakeLeaderboard();
            leaderboard.CalculateRankAndOrder(leaderboard.scores);
            return leaderboard;
        }

        public void Logout()
        {
            localUser = new FakeLocalUser();
        }
    }
}
