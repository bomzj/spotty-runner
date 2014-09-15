using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.SocialPlatforms;

namespace Assets.Scripts.Score.Fake
{
    class FakeScore : IScore
    {
        public FakeScore(string leaderboardID, string userID, int value)
        {
            this.leaderboardID = leaderboardID;
            _userID = userID;
            this.value = value;
        }

        public void ReportScore(Action<bool> callback)
        {
            throw new NotImplementedException();
        }

        public DateTime date
        {
            get { throw new NotImplementedException(); }
        }

        public string formattedValue
        {
            get { throw new NotImplementedException(); }
        }

        public string leaderboardID
        {
            get;
            set;
        }

        public int rank
        {
            get;
            set;
        }

        private string _userID;
        public string userID
        {
            get { return _userID; }
        }

        public long value
        {
            get;
            set;
        }
    }
}
