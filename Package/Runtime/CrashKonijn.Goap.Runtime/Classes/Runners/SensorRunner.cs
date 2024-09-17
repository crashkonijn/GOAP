using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using UnityEditor.Experimental.GraphView;

namespace CrashKonijn.Goap.Runtime
{
    public class SensorRunner : ISensorRunner
    {
        private SensorSet defaultSet = new();
        private Dictionary<IGoapAction, SensorSet> actionSets = new();
        private Dictionary<IGoal, SensorSet> goalSets = new();
        private Dictionary<string, SensorSet> goalsSets = new();
        private Dictionary<Type, ISensor> sensors = new();

        private IGlobalWorldData worldData;
        
        // Caching
        private List<Guid> keyCache = new ();

        public SensorRunner(
            IEnumerable<IWorldSensor> worldSensors,
            IEnumerable<ITargetSensor> targetSensors,
            IEnumerable<IMultiSensor> multiSensors,
            IGlobalWorldData globalWorldData
        ) {
            this.worldData = globalWorldData;
            
            foreach (var worldSensor in worldSensors)
            {
                this.defaultSet.AddSensor(worldSensor);
                
                this.sensors.TryAdd(worldSensor.Key.GetType(), worldSensor);
            }

            foreach (var targetSensor in targetSensors)
            {
                this.defaultSet.AddSensor(targetSensor);
                
                this.sensors.TryAdd(targetSensor.Key.GetType(), targetSensor);
            }
            
            foreach (var multiSensor in multiSensors)
            {
                this.defaultSet.AddSensor(multiSensor);
                
                foreach (var key in multiSensor.GetKeys())
                {
                    this.sensors.TryAdd(key, multiSensor);
                }
            }
        }

        public void Update()
        {
            foreach (var localSensor in this.defaultSet.LocalSensors)
            {
                localSensor.Update();
            }
        }

        public void Update(IGoapAction action)
        {
            var set = this.GetSet(action);
            
            foreach (var localSensor in set.LocalSensors)
            {
                localSensor.Update();
            }
        }

        public void Update(IGoal[] goals)
        {
            var set = this.GetSet(goals);
            
            foreach (var localSensor in set.LocalSensors)
            {
                localSensor.Update();
            }
        }

        public void SenseGlobal()
        {
            foreach (var globalSensor in this.defaultSet.GlobalSensors)
            {
                globalSensor.Sense(this.worldData);
            }
        }

        public void SenseGlobal(IGoapAction action)
        {
            var set = this.GetSet(action);
            
            foreach (var globalSensor in set.GlobalSensors)
            {
                globalSensor.Sense(this.worldData);
            }
        }

        public void SenseLocal(IMonoGoapActionProvider actionProvider)
        {
            foreach (var localSensor in this.defaultSet.LocalSensors)
            {
                localSensor.Sense(actionProvider.WorldData, actionProvider.Receiver, actionProvider.Receiver.Injector);
            }
        }
        
        public void SenseLocal(IMonoGoapActionProvider actionProvider, IGoapAction action)
        {
            if (actionProvider.IsNull())
                return;
            
            if (action == null)
                return;
            
            var set = this.GetSet(action);
            
            foreach (var localSensor in set.LocalSensors)
            {
                localSensor.Sense(actionProvider.WorldData, actionProvider.Receiver, actionProvider.Receiver.Injector);
            }
        }

        public void SenseLocal(IMonoGoapActionProvider actionProvider, IGoal goal)
        {
            if (actionProvider.IsNull())
                return;
            
            if (goal == null)
                return;
            
            var set = this.GetSet(goal);
            
            foreach (var localSensor in set.LocalSensors)
            {
                localSensor.Sense(actionProvider.WorldData, actionProvider.Receiver, actionProvider.Receiver.Injector);
            }
        }

        public void SenseLocal(IMonoGoapActionProvider actionProvider, IGoal[] goals)
        {
            if (actionProvider.IsNull())
                return;

            if (!goals.Any())
                return;
            
            var set = this.GetSet(goals);
            
            foreach (var localSensor in set.LocalSensors)
            {
                localSensor.Sense(actionProvider.WorldData, actionProvider.Receiver, actionProvider.Receiver.Injector);
            }
        }

        public void InitializeGraph(IGraph graph)
        {
            foreach (var rootNode in graph.RootNodes)
            {
                if (rootNode.Action is not IGoal goal)
                    continue;
                
                if (this.goalSets.ContainsKey(goal))
                    continue;
                
                var set = this.CreateSet(rootNode);
                this.goalSets[goal] = set;
            }
        }

        private SensorSet GetSet(IGoapAction action)
        {
            if (this.actionSets.TryGetValue(action, out var existingSet))
                return existingSet;
            
            return this.CreateSet(action);
        }

        private SensorSet GetSet(IGoal goal)
        {
            return this.goalSets.GetValueOrDefault(goal);
        }

        private SensorSet GetSet(IGoal[] goals)
        {
            var key = this.GetSetKey(goals);
            
            if (this.goalsSets.TryGetValue(key, out var existingSet))
                return existingSet;
            
            return this.CreateSet(goals);
        }
        
        private string GetSetKey(IGoal[] goals)
        {
            this.keyCache.Clear();
            
            foreach (var goal in goals)
            {
                this.keyCache.Add(goal.Guid);
            }

            return GuidCacheKey.GenerateKey(this.keyCache);
        }

        private SensorSet CreateSet(IGoapAction action)
        {
            var set = new SensorSet();
                
            foreach (var condition in action.Conditions)
            {
                set.Keys.Add(condition.WorldKey.GetType());
            }
            
            foreach (var key in set.Keys)
            {
                if (this.sensors.TryGetValue(key, out var sensor))
                {
                    set.AddSensor(sensor);
                }
            }
                
            this.actionSets[action] = set;

            return set;
        }
        
        private SensorSet CreateSet(IGoal[] goals)
        {
            var set = new SensorSet();
            
            foreach (var goal in goals)
            {
                var goalSet = this.GetSet(goal);
                set.Merge(goalSet);
            }
            
            this.goalsSets[this.GetSetKey(goals)] = set;

            return set;
        }

        private SensorSet CreateSet(INode node)
        {
            var actions = new List<IGoapAction>();
            node.GetActions(actions);
            
            var set = new SensorSet();
            
            foreach (var action in actions.Distinct())
            {
                var actionSet = this.GetSet(action);
                set.Merge(actionSet);
                
                if (action.Config.Target != null)
                {
                    set.Keys.Add(action.Config.Target.GetType());
                    set.AddSensor(this.sensors[action.Config.Target.GetType()]);
                }
            }

            return set;
        }
    }

    public class SensorSet
    {
        public HashSet<Type> Keys { get; } = new();
        public HashSet<ILocalSensor> LocalSensors { get; } = new();
        public HashSet<IGlobalSensor> GlobalSensors { get; } = new();
        
        public void AddSensor(ISensor sensor)
        {
            switch (sensor)
            {
                case IMultiSensor multiSensor:
                    this.LocalSensors.Add(multiSensor);
                    this.GlobalSensors.Add(multiSensor);
                    break;
                case ILocalSensor localSensor:
                    this.LocalSensors.Add(localSensor);
                    break;
                case IGlobalSensor globalSensor:
                    this.GlobalSensors.Add(globalSensor);
                    break;
            }
        }

        public void Merge(SensorSet set)
        {
            this.Keys.UnionWith(set.Keys);
            this.LocalSensors.UnionWith(set.LocalSensors);
            this.GlobalSensors.UnionWith(set.GlobalSensors);
        }
    }
}