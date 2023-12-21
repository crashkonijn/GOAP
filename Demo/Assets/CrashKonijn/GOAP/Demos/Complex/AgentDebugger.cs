using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Demos.Complex.Behaviours;

namespace CrashKonijn.Goap.Demos.Complex
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