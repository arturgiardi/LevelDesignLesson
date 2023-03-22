using System;
using System.Collections;
using UnityEngine;

namespace GEngine.Manager
{
    public class TimeManager : MonoBehaviour
    {
        float playTime;
        public float LoadTime { get; set; }
        const string playTimeKey = "playTimeKey";
        const string key = "Time";

        public void WaitAndDoAction(float delay, Action callback)
        {
            if (delay > 0)
                StartCoroutine(_WaitAndDoAction(delay, callback));
            else
                callback.Invoke();
        }

        public void WaitAFrameAndDoAction(Action callback)
        {
            StartCoroutine(_WaitAFrameAndDoAction(callback));
        }

        private IEnumerator _WaitAFrameAndDoAction(Action callback)
        {
            yield return null;
            callback.Invoke();
        }

        private IEnumerator _WaitAndDoAction(float delay, Action callback)
        {
            yield return new WaitForSeconds(delay);
            callback.Invoke();
        }

        public void WaitAndDoActionRealtime(float delay, Action callback)
        {
            if (delay > 0)
                StartCoroutine(_WaitAndDoActionRealtime(delay, callback));
            else
                callback.Invoke();
        }

        private IEnumerator _WaitAndDoActionRealtime(float delay, Action callback)
        {
            yield return new WaitForSecondsRealtime(delay);
            callback.Invoke();
        }
    }
}
