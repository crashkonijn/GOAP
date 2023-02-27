using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    [DefaultExecutionOrder(-100)]
    public class GoapRunnerBehaviour : MonoBehaviour, IGoapRunner
    {
        private Classes.Runners.GoapRunner runner;

        private void Awake()
        {
            this.runner = new Classes.Runners.GoapRunner();
        }

        public void Register(IGoapSet set) => this.runner.Register(set);

        private void Update()
        {
            this.runner.Run();
        }

        private void LateUpdate()
        {
            this.runner.Complete();
        }
        
        private void OnDestroy()
        {
            this.runner.Dispose();
        }
    }
}