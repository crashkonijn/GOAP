using System.Collections.Generic;
using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Classes.Controllers
{
    public class ReactiveController : IGoapController
    {
        private IGoap goap;
        private Dictionary<IAgentType, HashSet<IMonoAgent>> agents = new();

        public void Initialize(IGoap goap)
        {
            this.goap = goap;
            this.goap.Events.OnAgentResolve += this.OnAgentResolve;
            this.goap.Events.OnNoActionFound += this.OnNoActionFound;
        }

        public void Disable()
        {
            this.goap.Events.OnAgentResolve -= this.OnAgentResolve;
            this.goap.Events.OnNoActionFound -= this.OnNoActionFound;
        }

        public void OnUpdate()
        {
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
                
                // Update the action sensors for the agent
                agent.AgentType.SensorRunner.SenseLocal(agent, agent.ActionState.Action);

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

        private void OnNoActionFound(IAgent agent, IGoal goal)
        {
            this.GetQueue(agent.AgentType).Add(agent as IMonoAgent);
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