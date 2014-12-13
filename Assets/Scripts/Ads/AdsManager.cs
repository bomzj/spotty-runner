using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Assets.Scripts.Ads
{
    public class AdsManager
    {
        private IAdsProvider provider;

        public static IAdsProvider Current { get; private set; }

        public AdsManager(IAdsProvider provider)
        {
            this.provider = provider;
        }

        public IAd ShowInterstitialAd()
        {
            return provider.ShowInterstitialAd();
        }


    }
}
