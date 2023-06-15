using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{

    /// <summary>
    /// Helper class that allows you to execute code in the Unity main thread.
    /// </summary>
    public class UnityMainThreadDispatcher : MonoSingleton<UnityMainThreadDispatcher>
    {
        private static readonly Queue<Action> _executionQueue = new Queue<Action>();
        protected override void Awake()
        {
            if (instance == null)
            {
                instance = this as UnityMainThreadDispatcher;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }


        }

        /// <summary>
        /// Enqueues an action to be executed in the Unity main thread.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public void Enqueue(Action action)
        {
            lock (_executionQueue)
            {
                _executionQueue.Enqueue(action);
            }
        }

        private void Update()
        {
            while (true)
            {
                Action nextAction;

                lock (_executionQueue)
                {
                    if (_executionQueue.Count == 0)
                    {
                        break;
                    }

                    nextAction = _executionQueue.Dequeue();
                }

                nextAction();
            }
        }
    }


}
