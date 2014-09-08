using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.Ads
{
    public class FakeAd : IAd
    {
        public event EventHandler Clicked;

        public event EventHandler Closed;

        public FakeAd()
        {
            SimulateClosedEvent();
        }

        private void SimulateClosedEvent()
        {
            GameObject adGameObject = new GameObject("Ad");
            var ad = adGameObject.AddComponent<MonoBehaviour>();
            ad.StartCoroutine(StartClosing());
        }

        IEnumerator StartClosing()
        {
            Debug.Log("StartClosing");
            yield return new WaitForSeconds(2);
            OnClosed();
        }

        private void OnClosed()
        {
            if (Closed != null)
            {
                Closed.Invoke(this, new EventArgs());
                Debug.Log("Ad closed");
            }
        }
    }
}
