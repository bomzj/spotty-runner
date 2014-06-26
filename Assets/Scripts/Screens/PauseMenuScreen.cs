using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;


class PauseMenuScreen : GameScreen
{
    public PauseMenuScreen()
    {
        TransitionOnTime = TransitionOffTime = 0.5f;

        //backAction = new InputAction(
        //  new UnityEngine.KeyCode[] { UnityEngine.KeyCode.Escape },
        //  true);
    }

    public override void Activate()
    {
    }

      
    public override void HandleInput()
    {
        //foreach (Button button in buttons)
        //{
        //    button.HandleInput(gameTime, input);
        //}
    }

    //public override void Update(bool coveredByOtherScreen)
    //{
    //    base.Update(coveredByOtherScreen);
    //}
       
}