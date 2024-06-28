using System.Collections.Generic;

namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IAgentType
    {
        string Id { get; }
        IGoapConfig GoapConfig { get; }
        IAgentCollection Agents { get; }
        ISensorRunner SensorRunner { get; }
        IAgentTypeEvents Events { get; }
        IGlobalWorldData WorldData { get; }
        void Register(IMonoGoapActionProvider actionProvider);
        void Unregister(IMonoGoapActionProvider actionProvider);
        List<IConnectable> GetAllNodes();
        List<IGoapAction> GetActions();
        List<IGoal> GetGoals();

        TGoal ResolveGoal<TGoal>()
            where TGoal : IGoal;

        bool AllConditionsMet(IGoapAgent agent, IGoapAction action);
    }
}