using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameStateManagement
{
    /// <summary>
    /// The screen manager is a component which manages one or more GameScreen
    /// instances. It maintains a stack of screens, calls their Update and Draw
    /// methods at the appropriate times, and automatically routes input to the
    /// topmost active screen.
    /// </summary>
    public class ScreenManager
    {
        #region Fields

        List<GameScreen> screens = new List<GameScreen>();
        List<GameScreen> tempScreensList = new List<GameScreen>();

        #endregion

        #region Properties
        
        public Texture2D BlankTexture
        {
            get;
            private set;
        }

        private GameController gameController;

        #endregion

        #region Initialization

        public ScreenManager(GameController game)
        {
            this.gameController = game;
        }

        public void Initialize()
        {
            Texture2D blackTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            blackTexture.SetPixel(0, 0, Color.black);
            blackTexture.Apply();
        }

        #endregion

        #region Update and Draw


        /// <summary>
        /// Allows each screen to run logic.
        /// </summary>
        public void Update()
        {
            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others.
            tempScreensList.Clear();

            foreach (GameScreen screen in screens)
                tempScreensList.Add(screen);

            bool coveredByOtherScreen = false;

            // Loop as long as there are screens waiting to be updated.
            while (tempScreensList.Count > 0)
            {
                // Pop the topmost screen off the waiting list.
                GameScreen screen = tempScreensList[tempScreensList.Count - 1];

                tempScreensList.RemoveAt(tempScreensList.Count - 1);

                // Update the screen.
                //screen.Update(coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn ||
                    screen.ScreenState == ScreenState.Active)
                {
                    // If this is the first active screen we came across,
                    // give it a chance to handle input.
                    if (!coveredByOtherScreen)
                    {
                        //screen.HandleInput();
                    }

                    // If this is an active non-popup, inform any subsequent
                    // screens that they are covered by it.
                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }
        }

        #endregion

        #region Public Methods


        /// <summary>
        /// Adds a new screen to the screen manager.
        /// </summary>
        public void AddScreen(GameScreen screen)
        {
            screen.ScreenManager = this;
            screen.IsExiting = false;

            screen.Activate();
            
            lock (screens)
            {
                screens.Add(screen);
            }
        }


        /// <summary>
        /// Removes a screen from the screen manager. You should normally
        /// use GameScreen.ExitScreen instead of calling this directly, so
        /// the screen can gradually transition off rather than just being
        /// instantly removed.
        /// </summary>
        public void RemoveScreen(GameScreen screen)
        {
            // If we have a graphics device, tell the screen to unload content.
            screen.Unload();
            
            lock (screens)
            {
                screens.Remove(screen);
            }
            tempScreensList.Remove(screen);
        }


        /// <summary>
        /// Expose an array holding all the screens. We return a copy rather
        /// than the real master list, because screens should only ever be added
        /// or removed using the AddScreen and RemoveScreen methods.
        /// </summary>
        public GameScreen[] GetScreens()
        {
            return screens.ToArray();
        }


        /// <summary>
        /// Helper draws a translucent black fullscreen sprite, used for fading
        /// screens in and out, and for darkening the background behind popups.
        /// </summary>
        public void FadeBackBufferToBlack(float alpha)
        {
            //SpriteBatch.Draw(blankTexture, new Vector2(0,0), Color.Black * alpha);
        }

        #endregion
    }
}
