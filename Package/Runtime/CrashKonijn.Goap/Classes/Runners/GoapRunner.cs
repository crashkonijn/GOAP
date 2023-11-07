using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolver;
using CrashKonijn.Goap.Resolver.Models;

namespace CrashKonijn.Goap.Classes.Runners
{
    public class GoapRunner : IGoapRunner
    {
        private Dictionary<IAgentType, AgentTypeJobRunner> agentTypes = new();
        private Stopwatch stopwatch = new();

        public float RunTime { get; private set; }
        public float CompleteTime { get; private set; }

        public void Register(IAgentType agentType) => this.agentTypes.Add(agentType, new AgentTypeJobRunner(agentType, new GraphResolver(agentType.GetAllNodes().ToArray(), agentType.GoapConfig.KeyResolver)));

        public void Run()
        {
            this.stopwatch.Restart();
            
            foreach (var runner in this.agentTypes.Values)
            {
                runner.Run();
            }
            
            this.RunTime = this.GetElapsedMs();
                        
            foreach (var agent in this.Agents)
            {
                if (agent.IsNull())
                    continue;
                
                agent.Run();
            }
        }

        public void Complete()
        {
            this.stopwatch.Restart();
            
            foreach (var runner in this.agentTypes.Values)
            {
                runner.Complete();
            }
            
            this.CompleteTime = this.GetElapsedMs();
        }

        public void Dispose()
        {
            foreach (var runner in this.agentTypes.Values)
            {
                runner.Dispose();
            }
        }

        private float GetElapsedMs()
        {
            this.stopwatch.Stop();
            
            return (float) ((double)this.stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000);
        }

        public Graph GetGraph(IAgentType agentType) => this.agentTypes[agentType].GetGraph();
        public bool Knows(IAgentType agentType) => this.agentTypes.ContainsKey(agentType);
        public IMonoAgent[] Agents => this.agentTypes.Keys.SelectMany(x => x.Agents.All()).ToArray();

        public IAgentType[] AgentTypes => this.agentTypes.Keys.ToArray();
        
        public IAgentType GetAgentType(string id)
        {
            var agentTypes = this.AgentTypes.FirstOrDefault(x => x.Id == id);

            if (agentTypes == null)
                throw new KeyNotFoundException($"No agentType with id {id} found");

            return agentTypes;
        }

        public int QueueCount => this.agentTypes.Keys.Sum(x => x.Agents.GetQueueCount());
    }
}