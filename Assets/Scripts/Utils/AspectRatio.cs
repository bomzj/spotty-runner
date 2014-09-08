using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Classes.Utils
{
    /// <summary>
    /// Calculates scale factor based on current resolution to design resolution
    /// </summary>
    public class AspectRatio
    {
         public int ScreenDesignWidth { get; private set; }
        
        public int ScreenDesignHeight { get; private set; }

        public float DesignAspectRatio 
        { 
            get
            {
                return (float)ScreenDesignWidth / ScreenDesignHeight;
            }
        }

        public int ScreenWidth { get { return Screen.width; } }

        public int ScreenHeight { get { return Screen.height; } }

        public float ScreenAspectRatio
        {
            get 
            {
                return (float)ScreenWidth / ScreenHeight;
            }
        }

        public float Scale 
        {
            get
            {
                return CalculateScaleFactor();
            }
        }

        public AspectRatio(int screenDesignWidth, int screenDesignHeight)
	    {
            ScreenDesignWidth = screenDesignWidth;
            ScreenDesignHeight = screenDesignHeight;
	    }

        public float CalculateScaleFactor()
        {
            float targetaspect = DesignAspectRatio;

            // determine the game window's current aspect ratio
            float windowaspect = ScreenAspectRatio;

            // current viewport height should be scaled by this amount
            float scaleheight = windowaspect / targetaspect;

            float scale = 0;

            if (scaleheight < 1.0f)
            {
                scale = (float)ScreenWidth / ScreenDesignWidth;
            }
            else
            {
                scale = (float)ScreenHeight / ScreenDesignHeight;
            }

            return scale;
        }

        public void Calculate(Rect rect)
        {

        }
    }
}
