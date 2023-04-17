using System.Collections.Generic;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.Runners;
using CrashKonijn.Goap.Resolver.Interfaces;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IGoapSet
    {
        string Id { get; }
        GoapConfig GoapConfig { get; }
        IAgentCollection Agents { get; }
        ISensorRunner SensorRunner { get; }
        void Register(AgentBehaviour agent);
        void Unregister(AgentBehaviour agent);
        List<IAction> GetAllNodes();
        List<IActionBase> GetActions();

        TAction ResolveAction<TAction>()
            where TAction : ActionBase;

        TGoal ResolveGoal<TGoal>()
            where TGoal : IGoalBase;

        AgentDebugGraph GetDebugGraph();
    }
}