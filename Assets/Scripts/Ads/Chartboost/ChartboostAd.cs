using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Ads.Chartboost
{
    public class ChartboostAd : IAd
    {
        public event EventHandler Clicked;

        public event EventHandler Closed;

        public void OnClicked()
        {
            if (Clicked != null)
            {
                Clicked(this, EventArgs.Empty);
            }
        }

        public void OnClosed()
        {
            if (Closed != null)
            {
                Closed(this, EventArgs.Empty);
            }
        }
    }
}
