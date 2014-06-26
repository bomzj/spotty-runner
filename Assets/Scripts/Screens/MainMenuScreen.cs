using GameStateManagement;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace GameStateManagementSample
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : GameScreen
    {
        public MainMenuScreen()
        {
            TransitionOnTime= TransitionOffTime = 0.5f;
        }

        public override void Activate()
        {
           
        }

        #region Handle Input

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void Play_Clicked()
        {
            UnityEngine.Debug.Log("Play_Clicked");
            ScreenManager.AddScreen(new GameplayScreen());
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void Options_Clicked()
        {
           
        }

        void Credits_Clicked()
        {
        }

        void Exit_Clicked()
        {
#if UNITY_ANDROID
            UnityEngine.Application.Quit();
#endif
        }
      
        #endregion

        void UpdateScreenTransition()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Update title transition
            //Vector2 position = new Vector2(800 / 2, title.Height /2);
            //position.Y -= transitionOffset * title.Height;
            //title.Position = position;
            //title.Color = Color.White * TransitionAlpha;


            //// update each menu entry's location in turn
            //for (int i = 0; i < buttons.Count; i++)
            //{
            //    Button button = buttons[i];
            //    position = button.Position;

            //    // each entry is to be centered horizontally
            //    position.X = 800 / 2;

            //    if (ScreenState == ScreenState.TransitionOn)
            //        position.X -= transitionOffset * 256;
            //    else
            //        position.X += transitionOffset * 512;

            //    // set the entry's position
            //    button.Position = position;

            //    button.Color = Color.White * TransitionAlpha;
            //}
        }

        public override void HandleInput()
        {
        }

        //public override void Update(bool coveredByOtherScreen)
        //{
        //    UpdateScreenTransition();

        //    base.Update(coveredByOtherScreen);
        //}
    }
}

