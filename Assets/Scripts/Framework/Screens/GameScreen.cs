using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameStateManagement
{
    /// <summary>
    /// Enum describes the screen transition state.
    /// </summary>
    public enum ScreenState
    {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden,
    }


    /// <summary>
    /// A screen is a single layer that has update and draw logic, and which
    /// can be combined with other layers to build up a complex menu system.
    /// For instance the main menu, the options menu, the "are you sure you
    /// want to quit" message box, and the main game itself are all implemented
    /// as screens.
    /// </summary>
    public abstract class GameScreen : MonoBehaviour
    {
        /// <summary>
        /// Normally when one screen is brought up over the top of another,
        /// the first screen will transition off to make room for the new
        /// one. This property indicates whether the screen is only a small
        /// popup, in which case screens underneath it do not need to bother
        /// transitioning off.
        /// </summary>
        public bool IsPopup
        {
            get { return isPopup; }
            protected set { isPopup = value; }
        }

        bool isPopup = false;


        /// <summary>
        /// Indicates how long the screen takes to
        /// transition on when it is activated.
        /// </summary>
        public float TransitionOnTime
        {
            get { return transitionOnTime; }
            protected set { transitionOnTime = value; }
        }

        float transitionOnTime = 0;


        /// <summary>
        /// Indicates how long the screen takes to
        /// transition off when it is deactivated.
        /// </summary>
        public float TransitionOffTime
        {
            get { return transitionOffTime; }
            protected set { transitionOffTime = value; }
        }

        float transitionOffTime = 0;


        /// <summary>
        /// Gets the current position of the screen transition, ranging
        /// from zero (fully active, no transition) to one (transitioned
        /// fully off to nothing).
        /// </summary>
        public float TransitionPosition
        {
            get { return transitionPosition; }
            protected set { transitionPosition = value; }
        }

        float transitionPosition = 1;


        /// <summary>
        /// Gets the current alpha of the screen transition, ranging
        /// from 1 (fully active, no transition) to 0 (transitioned
        /// fully off to nothing).
        /// </summary>
        public float TransitionAlpha
        {
            get { return 1f - TransitionPosition; }
        }


        /// <summary>
        /// Gets the current screen transition state.
        /// </summary>
        public ScreenState ScreenState
        {
            get { return screenState; }
            protected set { screenState = value; }
        }

        ScreenState screenState = ScreenState.TransitionOn;


        /// <summary>
        /// There are two possible reasons why a screen might be transitioning
        /// off. It could be temporarily going away to make room for another
        /// screen that is on top of it, or it could be going away for good.
        /// This property indicates whether the screen is exiting for real:
        /// if set, the screen will automatically remove itself as soon as the
        /// transition finishes.
        /// </summary>
        public bool IsExiting
        {
            get { return isExiting; }
            protected internal set { isExiting = value; }
        }

        bool isExiting = false;

        /// <summary>
        /// Checks whether this screen is active and can respond to user input.
        /// </summary>
        public bool IsActive
        {
            get
            {
                return (screenState == ScreenState.TransitionOn ||
                        screenState == ScreenState.Active);
            }
        }

        /// <summary>
        /// Gets the manager that this screen belongs to.
        /// </summary>
        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            internal set { screenManager = value; }
        }

        ScreenManager screenManager;
       
        /// <summary>
        /// Activates the screen. Called when the screen is added to the screen manager or if the game resumes
        /// from being paused or tombstoned.
        /// </summary>
        /// <param name="instancePreserved">
        /// True if the game was preserved during deactivation, false if the screen is just being added or if the game was tombstoned.
        /// On Xbox and Windows this will always be false.
        /// </param>
        public virtual void Activate() { }

        
        /// <summary>
        /// Deactivates the screen. Called when the game is being deactivated due to pausing or tombstoning.
        /// </summary>
        public virtual void Deactivate() { }


        /// <summary>
        /// Unload content for the screen. Called when the screen is removed from the screen manager.
        /// </summary>
        public virtual void Unload() { }


        /// <summary>
        /// Allows the screen to run logic, such as updating the transition position.
        /// Unlike HandleInput, this method is called regardless of whether the screen
        /// is active, hidden, or in the middle of a transition.
        /// </summary>
        //public virtual void Update(bool coveredByOtherScreen)
        //{
        //    if (isExiting)
        //    {
        //        // If the screen is going away to die, it should transition off.
        //        screenState = ScreenState.TransitionOff;

        //        if (!UpdateTransition(transitionOffTime, 1))
        //        {
        //            // When the transition finishes, remove the screen.
        //            ScreenManager.RemoveScreen(this);
        //        }
        //    }
        //    else if (coveredByOtherScreen)
        //    {
        //        // If the screen is covered by another, it should transition off.
        //        if (UpdateTransition(transitionOffTime, 1))
        //        {
        //            // Still busy transitioning.
        //            screenState = ScreenState.TransitionOff;
        //        }
        //        else
        //        {
        //            // Transition finished!
        //            screenState = ScreenState.Hidden;
        //        }
        //    }
        //    else
        //    {
        //        // Otherwise the screen should transition on and become active.
        //        if (UpdateTransition(transitionOnTime, -1))
        //        {
        //            // Still busy transitioning.
        //            screenState = ScreenState.TransitionOn;
        //        }
        //        else
        //        {
        //            // Transition finished!
        //            screenState = ScreenState.Active;
        //        }
        //    }
        //}


        /// <summary>
        /// Helper for updating the screen transition position.
        /// </summary>
        bool UpdateTransition(float transitionTime, int direction)
        {
            // How much should we move by?
            float transitionDelta;

            if (transitionTime == 0)
                transitionDelta = 1;
            else
                transitionDelta = (float)(Time.deltaTime / transitionTime);

            // Update the transition position.
            transitionPosition += transitionDelta * direction;

            // Did we reach the end of the transition?
            if (((direction < 0) && (transitionPosition <= 0)) ||
                ((direction > 0) && (transitionPosition >= 1)))
            {
                transitionPosition = Mathf.Clamp(transitionPosition, 0, 1);
                return false;
            }

            // Otherwise we are still busy transitioning.
            return true;
        }


        /// <summary>
        /// Allows the screen to handle user input. Unlike Update, this method
        /// is only called when the screen is active, and not when some other
        /// screen has taken the focus.
        /// </summary>
        public virtual void HandleInput() 
        {
        }


        /// <summary>
        /// This is called when the screen should draw itself.
        /// </summary>
        public virtual void Draw() 
        { 
           
        }


        /// <summary>
        /// Tells the screen to go away. Unlike ScreenManager.RemoveScreen, which
        /// instantly kills the screen, this method respects the transition timings
        /// and will give the screen a chance to gradually transition off.
        /// </summary>
        public void ExitScreen()
        {
            if (TransitionOffTime == 0)
            {
                // If the screen has a zero transition time, remove it immediately.
                ScreenManager.RemoveScreen(this);
            }
            else
            {
                // Otherwise flag that it should transition off and then exit.
                isExiting = true;
            }
        }
    }
}
