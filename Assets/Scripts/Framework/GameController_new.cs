using GameStateManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Framework
{
    public class GameController_new : MonoBehaviour
    {
        private ScreenManager screenManager { get; set; }

        void Start()
        {
            //screenManager = new ScreenManager(this);
            //screenManager.Initialize();
            //screenManager.AddScreen(new SplashScreen());
            
            DontDestroyOnLoad(this.gameObject);
        }
    
        void Update()
        {
           // screenManager.Update();
        }
    }
}
