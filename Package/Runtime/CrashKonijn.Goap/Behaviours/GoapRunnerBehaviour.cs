using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Resolver.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace CrashKonijn.Goap.Behaviours
{
    [DefaultExecutionOrder(-100)]
    public class GoapRunnerBehaviour : MonoBehaviour, IGoapRunner
    {
        private Classes.Runners.GoapRunner runner;

        public float RunTime => this.runner.RunTime;
        public float CompleteTime => this.runner.CompleteTime;
        public int RunCount { get; private set; }

        public GoapConfigInitializerBase configInitializer;
        

        [FormerlySerializedAs("goapSetConfigFactories")]
        public List<AgentTypeFactoryBase> agentTypeConfigFactories = new();

        private GoapConfig config;
        private bool isInitialized = false;

        private void Awake()
        {
            this.Initialize();
        }

        public void Register(IAgentType agentType) => this.runner.Register(agentType);
        
        public void Register(IAgentTypeConfig agentTypeConfig) => this.runner.Register(new AgentTypeFactory(this.config).Create(agentTypeConfig));

        private void Update()
        {
            this.RunCount = this.runner.QueueCount;
            this.runner.Run();
        }

        private void LateUpdate()
        {
            this.runner.Complete();
        }
        
        private void OnDestroy()
        {
            this.runner.Dispose();
        }

        private void Initialize()
        {
            if (this.isInitialized)
                return;
            
            this.config = GoapConfig.Default;
            this.runner = new Classes.Runners.GoapRunner();
            
            if (this.configInitializer != null)
                this.configInitializer.InitConfig(this.config);
            
            this.CreateAgentTypes();
            
            this.isInitialized = true;
        }

        private void CreateAgentTypes()
        {
            var agentTypeFactory = new AgentTypeFactory(this.config);

            this.agentTypeConfigFactories.ForEach(factory =>
            {
                this.Register(agentTypeFactory.Create(factory.Create()));
            });
        }

        public IGraph GetGraph(IAgentType agentType) => this.runner.GetGraph(agentType);
        public bool Knows(IAgentType agentType) => this.runner.Knows(agentType);
        public IMonoAgent[] Agents => this.runner.Agents;
        public IAgentType[] AgentTypes => this.runner.AgentTypes;

        public IAgentType GetAgentType(string id)
        {
            this.Initialize();
            
            return this.runner.GetAgentType(id);
        }
    }
}