using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Ads.Fake
{
    public class FakeAdsProvider : IAdsProvider
    {

        public IAd ShowInterstitialAd()
        {
            return new FakeAd();
        }
    }
}
