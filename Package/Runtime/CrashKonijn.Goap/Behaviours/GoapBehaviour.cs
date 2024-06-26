using System.Collections.Generic;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Core.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace CrashKonijn.Goap.Behaviours
{
    [DefaultExecutionOrder(-100)]
    public class GoapBehaviour : MonoBehaviour, IGoap
    {
        private Classes.Runners.Goap goap;

        public float RunTime => this.goap.RunTime;
        public float CompleteTime => this.goap.CompleteTime;
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
        
        public void Register(IAgentType agentType) => this.goap.Register(agentType);
        
        public void Register(IAgentTypeConfig agentTypeConfig) => this.goap.Register(new AgentTypeFactory(this.config).Create(agentTypeConfig));

        private void Update()
        {
            this.goap.OnUpdate();
        }

        private void LateUpdate()
        {
            this.goap.OnLateUpdate();
        }
        
        private void OnDestroy()
        {
            this.goap?.Dispose();
        }

        private void Initialize()
        {
            if (this.isInitialized)
                return;
            
            this.config = GoapConfig.Default;
            
            if (this.configInitializer != null)
                this.configInitializer.InitConfig(this.config);

            var controller = this.GetComponent<IGoapController>();

            if (controller == null)
                throw new MissingComponentException("No IGoapController found on GameObject of GoapBehaviour");
            
            this.goap = new Classes.Runners.Goap(controller);
            
            controller.Initialize(this.goap);
            
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

        public IGoapEvents Events => this.goap.Events;
        public Dictionary<IAgentType, IAgentTypeJobRunner> AgentTypeRunners => this.goap.AgentTypeRunners;
        public IGoapController Controller => this.goap.Controller;
        public IGraph GetGraph(IAgentType agentType) => this.goap.GetGraph(agentType);
        public bool Knows(IAgentType agentType) => this.goap.Knows(agentType);
        public List<IMonoGoapAgent> Agents => this.goap.Agents;
        public IAgentType[] AgentTypes => this.goap.AgentTypes;

        public IAgentType GetAgentType(string id)
        {
            this.Initialize();
            
            return this.goap.GetAgentType(id);
        }
    }
}