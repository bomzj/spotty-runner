using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.SocialPlatforms;

namespace Assets.Scripts.Score.Fake
{
    class FakeLeaderboard : ILeaderboard
    {
        public const string LeaderboardID = "fake";

        private IEnumerable<string> userIDs;
        
        public FakeLeaderboard()
        {
            GenerateScores();
        }

        public void LoadScores(Action<bool> callback)
        {
            // Apply userIDs filter for score loading 
            callback(true);
        }

        public void SetUserFilter(string[] userIDs)
        {
            this.userIDs = userIDs;
        }

        public string id
        {
            get;
            set;
        }

        public bool loading
        {
            get { throw new NotImplementedException(); }
        }

        public IScore localUserScore
        {
            get { throw new NotImplementedException(); }
        }

        public uint maxRange
        {
            get { throw new NotImplementedException(); }
        }

        public Range range
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        private IEnumerable<IScore> _scores;
        public IScore[] scores
        {
            get 
            { 
                // Apply filter
                //if (userIDs != null && userIDs.Count() > 0)
                //{
                //    _scores.Where(s => s.userID == )
                //}
                return _scores.ToArray();
            }
            set { _scores = value; }
        }

        public TimeScope timeScope
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string title
        {
            get { throw new NotImplementedException(); }
        }

        public UserScope userScope
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<IScore> CalculateRankAndOrder(IEnumerable<IScore> scores)
        {
            var temp = scores.ToList();
            // Order by score value
            temp = temp.OrderByDescending(s => s.value).ToList();
            for (int i=0; i< temp.Count(); i++)
            {
                FakeScore fakeScore = temp[i] as FakeScore;
                fakeScore.rank = i + 1;
            }
            return temp;
        }

        private void GenerateScores()
        {
            scores = new FakeScore[] 
                { 
                    new FakeScore(LeaderboardID, "fake1", 9),
                    new FakeScore(LeaderboardID, "fake2", 8),
                    new FakeScore(LeaderboardID, "fake3", 7),
                    new FakeScore(LeaderboardID, "fake4", 6),
                    new FakeScore(LeaderboardID, "fake5", 5),
                    new FakeScore(LeaderboardID, "fake6", 4),
                    new FakeScore(LeaderboardID, "fake7", 3),
                    new FakeScore(LeaderboardID, "fake8", 3),
                };
        }
    }
}
