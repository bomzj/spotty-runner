using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Classes.PausableCoroutine
{
    public class CoroutineController
    {
        private IEnumerator _routine;

        public CoroutineState state;

        public CoroutineController(IEnumerator routine)
        {
            _routine = routine;
            state = CoroutineState.Ready;
        }

        public IEnumerator Start()
        {
            if (state != CoroutineState.Ready)
            {
                throw new System.InvalidOperationException("Unable to start coroutine in state: " + state);
            }

            state = CoroutineState.Running;
            while (_routine.MoveNext())
            {
                yield return _routine.Current;
                while (state == CoroutineState.Paused)
                {
                    yield return null;
                }
                if (state == CoroutineState.Finished)
                {
                    yield break;
                }
            }

            state = CoroutineState.Finished;
        }

        public void Stop()
        {
            if (state != CoroutineState.Running  || state != CoroutineState.Paused)
            {
                throw new System.InvalidOperationException("Unable to stop coroutine in state: " + state);
            }
 
            state = CoroutineState.Finished;
        }

        public void Pause()
        {
            if (state != CoroutineState.Running)
            {
                throw new System.InvalidOperationException("Unable to pause coroutine in state: " + state);
            }

            state = CoroutineState.Paused;
        }

        public void Resume()
        {
            if (state != CoroutineState.Paused)
            {
                throw new System.InvalidOperationException("Unable to resume coroutine in state: " + state);
            }

            state = CoroutineState.Running;
        }
    }
}
