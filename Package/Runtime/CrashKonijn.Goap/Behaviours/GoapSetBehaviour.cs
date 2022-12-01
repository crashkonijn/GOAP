using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.Runners;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Scriptables;
using LamosInteractive.Goap;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    public interface IGoapSet
    {
        void Initialize(GoapConfig config);
        void Register(AgentBehaviour agent);
        void Unregister(AgentBehaviour agent);
        void Run(HashSet<IMonoAgent> agents);

        TAction ResolveAction<TAction>()
            where TAction : ActionBase;

        TGoal ResolveGoal<TGoal>()
            where TGoal : IGoalBase;

        AgentDebugGraph GetDebugGraph();
    }

    [DefaultExecutionOrder(-99)]
    public class GoapSetBehaviour : MonoBehaviour, IGoapSet
    {
        [SerializeField]
        public GoapRunnerBehaviour goapRunner;
        [SerializeField]
        public GoapSetConfigScriptable config;

        private GoapSetRunner runner;

        private HashSet<IGoalBase> goals;
        private HashSet<ActionBase> actions;
        private GraphResolver graphResolver;
        private SensorRunner sensorRunner;
        public  GoapConfig goapConfig;
        
        // GC cache
        private LocalWorldData localData;

        private void Awake()
        {
            Debug.Log("Awake");
            this.runner = new GoapSetRunner(this.config, this.goapRunner);
        }

        public void Initialize(GoapConfig config) => this.runner.Initialize(config);
        public void Register(AgentBehaviour agent) => this.runner.Register(agent);
        public void Unregister(AgentBehaviour agent) => this.runner.Unregister(agent);
        public void Run(HashSet<IMonoAgent> agents) => this.runner.Run(agents);
        public TAction ResolveAction<TAction>() where TAction : ActionBase => this.runner.ResolveAction<TAction>();
        public TGoal ResolveGoal<TGoal>() where TGoal : IGoalBase => this.runner.ResolveGoal<TGoal>();
        public AgentDebugGraph GetDebugGraph() => this.runner.GetDebugGraph();
    }
}