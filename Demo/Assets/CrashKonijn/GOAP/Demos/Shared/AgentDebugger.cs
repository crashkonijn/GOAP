using CrashKonijn.Goap.Interfaces;
using Demos.Shared.Behaviours;

namespace Demos.Shared
{
    public class AgentDebugger : IAgentDebugger
    {
        public string GetInfo(IMonoAgent agent, IComponentReference references)
        {
            var hunger = references.GetCachedComponent<HungerBehaviour>();
            
            return $"Hunger: {hunger.hunger}";
        }
    }
}