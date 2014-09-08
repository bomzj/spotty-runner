using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Score
{
    public class Leaderboard
    {
        public IEnumerable<Score> GetBestResults(int numberOfResults)
        {
            return null;
        }

        public int GetPlayerRank(string playerID)
        {
            return 0;
        }

        public IEnumerable<Score> GetResults(int rankFrom, int rankTo)
        {
            return null;
        }
        
    }
}
