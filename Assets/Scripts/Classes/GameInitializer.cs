using Assets.Scripts.Classes.Core;
using Assets.Scripts.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    /// <summary>
    /// Initialize game with custom settings, services and other needed stuff
    /// </summary>
    public class GameInitializer : MonoBehaviour
    {
        void Start()
        {
            InitializeGame();
        }

        public void InitializeGame()
        {
            InitializeGameServices();
            Debug.Log("Game initialized");
        }

        private void InitializeGameServices()
        {
            GameServices.AddService(new ObjectPool());
            GameServices.AddService(new MessageBus());
            Debug.Log("Game services initialized");
        }
    }
}
