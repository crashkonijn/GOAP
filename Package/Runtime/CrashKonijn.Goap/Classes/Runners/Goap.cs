using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Resolver;

namespace CrashKonijn.Goap.Classes.Runners
{
    public class Goap : IGoap
    {
        public IGoapEvents Events { get; } = new GoapEvents();
        public Dictionary<IAgentType, IAgentTypeJobRunner> AgentTypeRunners { get; private set; } = new();
        private Stopwatch stopwatch = new();

        public float RunTime { get; private set; }
        public float CompleteTime { get; private set; }

        public IGoapController Controller { get; }

        public Goap(IGoapController controller)
        {
            this.Controller = controller;
        }
        
        public void Register(IAgentType agentType) {
            this.AgentTypeRunners.Add(agentType, new AgentTypeJobRunner(agentType, new GraphResolver(agentType.GetAllNodes().ToArray(), agentType.GoapConfig.KeyResolver)));

            agentType.Events.Bind(this.Events);
            
            this.Events.AgentTypeRegistered(agentType);
        }

        public void OnUpdate()
        {
            this.Controller.OnUpdate();
        }

        public void OnLateUpdate()
        {
            this.Controller.OnLateUpdate();
        }

        public void Dispose()
        {
            foreach (var runner in this.AgentTypeRunners.Values)
            {
                runner.Dispose();
            }
        }

        private float GetElapsedMs()
        {
            this.stopwatch.Stop();
            
            return (float) ((double)this.stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000);
        }

        public IGraph GetGraph(IAgentType agentType) => this.AgentTypeRunners[agentType].GetGraph();
        public bool Knows(IAgentType agentType) => this.AgentTypeRunners.ContainsKey(agentType);
        public IMonoAgent[] Agents => this.AgentTypeRunners.Keys.SelectMany(x => x.Agents.All()).ToArray();

        public IAgentType[] AgentTypes => this.AgentTypeRunners.Keys.ToArray();
        
        public IAgentType GetAgentType(string id)
        {
            var agentTypes = this.AgentTypes.FirstOrDefault(x => x.Id == id);

            if (agentTypes == null)
                throw new KeyNotFoundException($"No agentType with id {id} found");

            return agentTypes;
        }
    }
}