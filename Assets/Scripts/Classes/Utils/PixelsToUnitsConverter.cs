using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuslikGames.SpottyRunner.Assets.Scripts.Classes.Utils
{
    class PixelsToUnitsConverter
    {
        private const float PixelsToUnit = 100;
        
        public static float PixelsToUnits(int pixels)
        {
            return pixels / PixelsToUnit;
        }
    }
    

}
