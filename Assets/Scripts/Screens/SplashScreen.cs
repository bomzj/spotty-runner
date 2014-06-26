using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using GameStateManagementSample;
using UnityEngine;

class SplashScreen : GameScreen
{
    float showTime;
    float elapsedTime;
    float pauseAlpha;

    public SplashScreen()
    {
        showTime = 2;
        TransitionOnTime = 3;
        TransitionOffTime = 2;
    }

    public override void Activate()
    {
    }

    public override void Unload()
    {
        ScreenManager.AddScreen(new MainMenuScreen());
    }

    public override void HandleInput()
    {
        // Hanlder back button
        //if (backAction.Evaluate(input))
        {
            UnityEngine.Application.Quit();
        }
    }

    //public override void Update(bool coveredByOtherScreen)
    //{
    //    if (ScreenState == GameStateManagement.ScreenState.Active)
    //    {
    //        if (elapsedTime > showTime)
    //        {
    //            this.ExitScreen();
    //        }

    //        elapsedTime += Time.deltaTime;
    //    }

    //    base.Update(coveredByOtherScreen);
    //}

    public override void Draw()
    {
        //ScreenManager.FadeBackBufferToBlack(1f);
            
        //float alpha = (float)Math.Pow(TransitionAlpha, 2);
        //ScreenManager.FadeBackBufferToBlack(1f - alpha);
    }
}

