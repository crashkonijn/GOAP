using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Scriptables;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    [DefaultExecutionOrder(-99)]
    public class GoapSetBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GoapSetConfigScriptable config;

        [SerializeField]
        private GoapRunnerBehaviour runner;

        [System.Obsolete("'Set' is deprecated, please use 'GoapSet' instead.   Exact same functionality, name changed to mitigate confusion with the word 'set' which could have many meanings.")]
        public IGoapSet Set { get; private set; }

        public IGoapSet GoapSet { get; private set; }

        private void Awake()
        {
            var goapSet = new GoapSetFactory(GoapConfig.Default).Create(this.config);

            this.runner.Register(goapSet);
            
            this.GoapSet = goapSet;
        }
    }
}