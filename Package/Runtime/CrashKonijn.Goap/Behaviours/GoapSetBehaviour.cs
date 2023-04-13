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

        public IGoapSet Set { get; private set; }

        private void Awake()
        {
            var set = new GoapSetFactory(GoapConfig.Default).Create(this.config);

            this.runner.Register(set);
            
            this.Set = set;
        }
    }
}