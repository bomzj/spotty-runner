using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Classes.PausableCoroutine;
using System;
using Assets.Scripts.Classes.Extensions;
using System.Linq;
public class Pause : MonoBehaviour {

    [HideInInspector]
    public bool isPaused;
    private IEnumerable<MonoBehaviour> scripts;

    private List<CoroutineController> runningCoroutines = new List<CoroutineController>();

    // Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        RemoveFinishedCoroutines();
        
	}

    public void SetPause()
    {
        SetPause(true);
        
    }

    public void SetPause(bool pause)
    {
       // Get all attached scripts 
        scripts = this.gameObject.GetComponents<MonoBehaviour>();
        // Exclude pause script
        scripts = scripts.Except(new[] { this });

        // Disable all scripts attached to this game object
        foreach (var item in scripts)
        {
            item.enabled = !pause;
        }
        
        // Pause all pausable coroutines are running
        var runningCoroutinesCopy = runningCoroutines.ToArray();
        foreach (var item in runningCoroutinesCopy)
        {
            if (pause && item.state == CoroutineState.Running)
            {
                item.Pause();
            }
            else if (!pause && item.state == CoroutineState.Paused)
            {
                item.Resume();
            }
        }

        // Pause all animations for this game object

        // Set pause flag
        isPaused = pause;
    }

    public void Resume()
    {
        SetPause(false);
    }

    public void StartPausableCoroutine(IEnumerator coroutine)
    {
        var controller = (this as MonoBehaviour).StartPausableCoroutine(coroutine);
        runningCoroutines.Add(controller);
        if (isPaused)
        {
            controller.Pause();
        }
    }

    private void RemoveFinishedCoroutines()
    {
        // Remove finished coroutines
        if (runningCoroutines.Count > 0)
        {
            var copyCoroutines = runningCoroutines.ToArray();
            foreach (var item in copyCoroutines)
            {
                if (item.state == CoroutineState.Finished)
                {
                    runningCoroutines.Remove(item);
                }
            }
        }
    }
}
