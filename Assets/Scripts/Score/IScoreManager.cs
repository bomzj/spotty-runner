using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Score
{
    public interface IScoreManager
    {
        void Submit(string playerID, int scoreCount);

        Score GetPlayerScore(string playerID);
       
    }
}
