using System.Collections.Generic;
using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Classes.Controllers
{
    public class ReactiveController : IGoapController
    {
        private IGoap goap;
        private Dictionary<IAgentType, HashSet<IMonoGoapAgent>> agents = new();

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
                agent.AgentType.SensorRunner.SenseLocal(agent, agent.Agent.ActionState.Action as IGoapAction);

                agent.Agent.Run();
            }
        }

        public void OnLateUpdate()
        {
            foreach (var runner in this.goap.AgentTypeRunners.Values)
            {
                runner.Complete();
            }
        }

        private void OnNoActionFound(IMonoGoapAgent agent, IGoal goal)
        {
            this.GetQueue(agent.AgentType).Add(agent);
        }

        private void OnAgentResolve(IMonoGoapAgent agent)
        {
            this.GetQueue(agent.AgentType).Add(agent);
        }
        
        private HashSet<IMonoGoapAgent> GetQueue(IAgentType agentType)
        {
            if (!this.agents.ContainsKey(agentType))
                this.agents.Add(agentType, new HashSet<IMonoGoapAgent>());
            
            return this.agents[agentType];
        }
    }
}