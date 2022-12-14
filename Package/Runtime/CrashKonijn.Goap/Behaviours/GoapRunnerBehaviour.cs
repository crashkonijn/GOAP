using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Observers;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    [DefaultExecutionOrder(-100)]
    public class GoapRunnerBehaviour : MonoBehaviour, IGoapRunner
    {
        private Classes.Runners.GoapRunner runner;

        private void Awake()
        {
            this.runner = new Classes.Runners.GoapRunner(
                GoapConfig.Default
            );
        }

        public void Register(IGoapSet set) => this.runner.Register(set);

        private void FixedUpdate()
        {
            this.runner.Run();
        }
    }
}