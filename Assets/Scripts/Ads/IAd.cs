using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Ads
{
    public interface IAd
    {
        event EventHandler Clicked;
        event EventHandler Closed;
    }
}
