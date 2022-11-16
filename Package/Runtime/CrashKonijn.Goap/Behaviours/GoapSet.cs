﻿using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.Runners;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Scriptables;
using LamosInteractive.Goap;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    public class GoapSet : MonoBehaviour
    {
        [SerializeField]
        public GoapResolver goapResolver;
        [SerializeField]
        public GoapSetConfig config;

        private HashSet<IGoalBase> goals;
        private HashSet<ActionBase> actions;
        private GraphResolver graphResolver;
        private SensorRunner sensorRunner;
        public  GoapConfig goapConfig;
        
        // GC cache
        private LocalWorldData localData;

        private void Awake()
        {
           this.actions = this.config.actions.ToHashSet();
           this.goals = new ClassResolver().Load(this.config.goals);
            
            // this.goals = this.GetComponentsInChildren<IGoalBase>().ToHashSet();
            // this.actions = this.GetComponentsInChildren<IActionBase>().ToHashSet();
            this.goapResolver.Register(this);

            this.GatherSensors();
        }

        private void GatherSensors()
        {
            var worldSensors = new ClassResolver().Load(this.config.worldSensors);
            var targetSensors = new ClassResolver().Load(this.config.targetSensors);

            this.sensorRunner = new SensorRunner(worldSensors, targetSensors);
        }
        
        public void Initialize(GoapConfig config)
        {
            this.goapConfig = config;
            var mixed = this.goals.Concat(this.actions.Cast<LamosInteractive.Goap.Interfaces.IAction>()).ToHashSet();
            
            this.graphResolver = new GraphResolver(
                actions: mixed, 
                conditionObserver: config.ConditionObserver, 
                costObserver: config.CostObserver, 
                keyResolver: config.KeyResolver
            );
        }

        public void Register(Agent agent) => this.goapResolver.Register(agent);
        public void Unregister(Agent agent) => this.goapResolver.Unregister(agent);

        public void Run(HashSet<Agent> agents)
        {
            var globalData = this.sensorRunner.SenseGlobal();

            foreach (var agent in agents)
            {
                this.Run(globalData, agent);
            }
        }

        private void Run(GlobalWorldData globalData, Agent agent)
        {
            if (agent.CurrentGoal == null)
                return;

            if (agent.CurrentAction != null)
                return;
            
            this.localData = this.sensorRunner.SenseLocal(globalData, agent);
            this.InjectData(this.localData);
            
            agent.SetWorldData(this.localData);
            
            var result = this.graphResolver.Resolve(agent.CurrentGoal);
            var action = result.Action as IActionBase;

            if (action == null)
            {
                UnityEngine.Debug.Log("No action found");
                return;
            }
            
            agent.SetAction(action, result.Path.OfType<IActionBase>().ToList(), this.localData.GetTarget(action));
        }

        private void InjectData(LocalWorldData worldData)
        {
            this.goapConfig.ConditionObserver.SetWorldData(worldData);
            this.goapConfig.CostObserver.SetWorldData(worldData);
            this.goapConfig.KeyResolver.SetWorldData(worldData);
        }

        public TAction ResolveAction<TAction>()
            where TAction : ActionBase
        {
            var result = this.actions.FirstOrDefault(x => x.GetType() == typeof(TAction));

            if (result != null)
                return (TAction) result;
            
            throw new KeyNotFoundException($"No action found of type {typeof(TAction)}");
        }

        public TGoal ResolveGoal<TGoal>()
            where TGoal : IGoalBase
        {
            var result = this.goals.FirstOrDefault(x => x.GetType() == typeof(TGoal));

            if (result != null)
                return (TGoal) result;
            
            throw new KeyNotFoundException($"No action found of type {typeof(TGoal)}");
        }

        public AgentDebugGraph GetDebugGraph()
        {
            return new AgentDebugGraph
            {
                Goals = this.goals,
                Actions = this.actions,
                Config = this.goapConfig
            };
        }
    }
}