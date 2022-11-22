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
                this.GetComponent<AgentCollection>(),
                new GoapConfig(
                    this.GetComponent<ICostObserver>(),
                    this.GetComponent<IConditionObserver>(),
                    this.GetComponent<IKeyResolver>()
                )
            );
        }

        private void Start()
        {
            this.runner.Initialize();
        }

        public void Register(IGoapSet set) => this.runner.Register(set);
        public void Register(IMonoAgent agent) => this.runner.Register(agent);
        public void Unregister(IMonoAgent agent) => this.runner.Unregister(agent);

        private void FixedUpdate()
        {
            this.runner.Run();
        }
    }
}