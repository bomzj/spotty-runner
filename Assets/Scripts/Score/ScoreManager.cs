using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Score
{
    public class ScoreManager : IScoreManager
    {
        public void Submit(string playerID, int scoreCount)
        {

        }

        public Score GetPlayerScore(string playerID)
        {
            return new Score() { PlayerID="keke", ScoreCount=0};
        }
    }
}
