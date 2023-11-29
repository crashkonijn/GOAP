using System.Collections.Generic;
using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Classes.Controllers
{
    public class ProactiveController : IGoapController
    {
        private IGoap goap;
        private Dictionary<IAgentType, HashSet<IMonoAgent>> agents = new();
        
        public float ResolveTime { get; set; } = 1f;

        public void Initialize(IGoap goap)
        {
            this.goap = goap;
            this.goap.Events.OnAgentResolve += this.OnAgentResolve;
        }

        public void Disable()
        {
            this.goap.Events.OnAgentResolve -= this.OnAgentResolve;
        }

        public void OnUpdate()
        {
            foreach (var agent in this.goap.Agents)
            {
                if (agent.IsNull())
                    continue;

                if (agent.Timers.Resolve.IsRunningFor(this.ResolveTime))
                {
                    agent.ResolveAction();
                }
            }
            
            foreach (var (type, runner) in this.goap.AgentTypeRunners)
            {
                var queue = this.GetQueue(type);
                
                runner.Run(queue);
                
                queue.Clear();
            }
            
            foreach (var agent in this.goap.Agents)
            {
                if (agent.IsNull())
                    continue;
                
                agent.Run();
            }
        }

        public void OnLateUpdate()
        {
            foreach (var runner in this.goap.AgentTypeRunners.Values)
            {
                runner.Complete();
            }
        }

        private void OnAgentResolve(IAgent agent)
        {
            this.GetQueue(agent.AgentType).Add(agent as IMonoAgent);
        }
        
        private HashSet<IMonoAgent> GetQueue(IAgentType agentType)
        {
            if (!this.agents.ContainsKey(agentType))
                this.agents.Add(agentType, new HashSet<IMonoAgent>());
            
            return this.agents[agentType];
        }
    }
}