using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuslikGames.SpottyRunner.Classes.Extensions
{
    public static class RandomExtensions
    {
        public static double Range(this Random random, double minimum, double maximum)
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
