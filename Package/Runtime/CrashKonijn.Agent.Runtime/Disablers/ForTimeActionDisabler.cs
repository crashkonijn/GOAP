using CrashKonijn.Agent.Core;
using UnityEngine;

namespace CrashKonijn.Agent.Runtime
{
    public class ForTimeActionDisabler : IActionDisabler
    {
        private readonly float enableAt;

        public ForTimeActionDisabler(float time)
        {
            this.enableAt = Time.time + time;
        }

        public bool IsDisabled(IAgent agent)
        {
            return Time.time < this.enableAt;
        }
    }
}