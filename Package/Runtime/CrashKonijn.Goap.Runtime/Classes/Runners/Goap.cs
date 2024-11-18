using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Resolver;

namespace CrashKonijn.Goap.Runtime
{
    public class Goap : IGoap
    {
        private Stopwatch stopwatch = new();

        /** GC caches **/
        private List<IMonoGoapActionProvider> agentsGC = new();

        /** **/

        public IGoapEvents Events { get; } = new GoapEvents();

        public Dictionary<IAgentType, IAgentTypeJobRunner> AgentTypeRunners { get; private set; } = new();

        public float RunTime { get; private set; }
        public float CompleteTime { get; private set; }

        public IGoapController Controller { get; }

        public Goap(IGoapController controller)
        {
            this.Controller = controller;
            this.Config = GoapConfig.Default;
        }

        public void Register(IAgentType agentType)
        {
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

            return (float) ((double) this.stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000);
        }

        public IGraph GetGraph(IAgentType agentType) => this.AgentTypeRunners[agentType].GetGraph();
        public bool Knows(IAgentType agentType) => this.AgentTypeRunners.ContainsKey(agentType);

        public List<IMonoGoapActionProvider> Agents
        {
            get
            {
                this.agentsGC.Clear();

                foreach (var runner in this.AgentTypeRunners.Keys)
                {
                    this.agentsGC.AddRange(runner.Agents.All());
                }

                return this.agentsGC;
            }
        }

        public IAgentType[] AgentTypes => this.AgentTypeRunners.Keys.ToArray();
        public IGoapConfig Config { get; }

        public IAgentType GetAgentType(string id)
        {
            var agentTypes = this.AgentTypes.FirstOrDefault(x => x.Id == id);

            if (agentTypes == null)
                throw new KeyNotFoundException($"No agentType with id {id} found");

            return agentTypes;
        }
    }
}
