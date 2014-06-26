using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SuslikGames.SpottyRunner
{
    class Move : MonoBehaviour
    {
        public Vector2 speed = new Vector2(-3f, 0);

        #region Unity Callbacks

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 movement = speed * Time.deltaTime;
            this.transform.Translate(movement);
        }

        #endregion 
    }
}
