using System.Collections.Generic;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.Runners;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Scriptables;
using LamosInteractive.Goap.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    public interface IGoapSet
    {
        GoapConfig GoapConfig { get; }
        IAgentCollection Agents { get; }
        SensorRunner SensorRunner { get; }
        void Register(AgentBehaviour agent);
        void Unregister(AgentBehaviour agent);
        List<IAction> GetAllNodes();

        TAction ResolveAction<TAction>()
            where TAction : ActionBase;

        TGoal ResolveGoal<TGoal>()
            where TGoal : IGoalBase;

        AgentDebugGraph GetDebugGraph();
    }

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