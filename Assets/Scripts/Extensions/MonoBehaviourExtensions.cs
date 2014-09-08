using Assets.Scripts.Classes.PausableCoroutine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Classes.Extensions
{
    public static class MonoBehaviourExtensions
    {
        /// <summary>
        /// Starts pausable coroutine which can be paused by Pause script
        /// </summary>
        /// <param name="monoBehaviour"></param>
        /// <param name="coroutine"></param>
        public static CoroutineController StartPausableCoroutine(this MonoBehaviour monoBehaviour, IEnumerator coroutine)
        {
            if (coroutine == null)
            {
                throw new System.ArgumentNullException("routine");
            }

            var coroutineController = new CoroutineController(coroutine);
            monoBehaviour.StartCoroutine(coroutineController.Start());
            return coroutineController;
        }
    }
}
