using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using LamosInteractive.Goap;

namespace CrashKonijn.Goap.Classes.Runners
{
    public class GoapSetRunner
    {
        private readonly Dictionary<IGoapSet, GraphResolver> graphResolvers = new();

        // GC cache
        private LocalWorldData localData;

        public void Run(IGoapSet set)
        {
            var globalData = set.SensorRunner.SenseGlobal();

            foreach (var agent in set.Agents.All())
            {
                this.Run(set, globalData, agent);
            }
        }

        private void Run(IGoapSet set, GlobalWorldData globalData, IMonoAgent agent)
        {
            if (agent.CurrentGoal == null)
                return;

            if (agent.CurrentAction != null)
                return;
            
            this.localData = set.SensorRunner.SenseLocal(globalData, agent);
            this.InjectData(set.GoapConfig, this.localData);
            
            agent.SetWorldData(this.localData);
            
            var result = this.GetGraphResolver(set).Resolve(agent.CurrentGoal);
            var action = result.Action as IActionBase;

            if (action == null)
            {
                UnityEngine.Debug.Log("No action found");
                return;
            }
            
            agent.SetAction(action, result.Path.OfType<IActionBase>().ToList(), this.localData.GetTarget(action));
        }

        private void InjectData(GoapConfig config, LocalWorldData worldData)
        {
            config.ConditionObserver.SetWorldData(worldData);
            config.CostObserver.SetWorldData(worldData);
            config.KeyResolver.SetWorldData(worldData);
        }

        private GraphResolver GetGraphResolver(IGoapSet set)
        {
            if (this.graphResolvers.TryGetValue(set, out var resolver))
            {
                return resolver;
            }

            resolver = new GraphResolver(
                set.GetAllNodes(),
                set.GoapConfig.ConditionObserver,
                set.GoapConfig.CostObserver,
                set.GoapConfig.KeyResolver
            );
            
            this.graphResolvers.Add(set, resolver);

            return resolver;
        }
    }
}