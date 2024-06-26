using CrashKonijn.Goap.Core.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    public class Timer : ITimer
    {
        private float lastTouch = 0;
        
        public void Touch()
        {
            this.lastTouch = Time.realtimeSinceStartup;
        }
        
        public float GetElapsed()
        {
            return Time.realtimeSinceStartup - this.lastTouch;
        }
        
        public bool IsRunningFor(float time)
        {
            return this.GetElapsed() >= time;
        }
    }
}