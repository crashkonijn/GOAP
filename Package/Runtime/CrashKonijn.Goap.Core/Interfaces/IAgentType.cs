using System.Collections.Generic;

namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IAgentType
    {
        string Id { get; }
        IGoapConfig GoapConfig { get; }
        IAgentCollection Agents { get; }
        ISensorRunner SensorRunner { get; }
        IAgentDebugger Debugger { get; }
        IAgentTypeEvents Events { get; }
        void Register(IMonoAgent agent);
        void Unregister(IMonoAgent agent);
        List<IConnectable> GetAllNodes();
        List<IAction> GetActions();
        List<IGoal> GetGoals();

        TAction ResolveAction<TAction>()
            where TAction : IAction;

        TGoal ResolveGoal<TGoal>()
            where TGoal : IGoal;

        IAgentDebugGraph GetDebugGraph();
    }
}