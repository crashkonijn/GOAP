using System.Collections.Generic;
using CrashKonijn.Goap.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace CrashKonijn.Goap.Runtime
{
    [DefaultExecutionOrder(-100)]
    public class GoapBehaviour : MonoBehaviour, IGoap
    {
        private Goap goap;

        public float RunTime => this.goap.RunTime;
        public float CompleteTime => this.goap.CompleteTime;
        public int RunCount { get; private set; }

        public GoapConfigInitializerBase configInitializer;

        [FormerlySerializedAs("goapSetConfigFactories")]
        public List<AgentTypeFactoryBase> agentTypeConfigFactories = new();

        private bool isInitialized = false;

        private void Awake()
        {
            this.Initialize();
        }

        public void Register(IAgentType agentType) => this.goap.Register(agentType);

        public void Register(IAgentTypeConfig agentTypeConfig) => this.goap.Register(new AgentTypeFactory(this.Config).Create(agentTypeConfig));

        private void Update()
        {
            this.goap?.OnUpdate();
        }

        private void LateUpdate()
        {
            this.goap?.OnLateUpdate();
        }

        private void OnDestroy()
        {
            this.goap?.Dispose();
        }

        private void Initialize()
        {
            if (this.isInitialized)
                return;

            var controller = this.GetComponent<IGoapController>();

            if (controller == null)
                throw new MissingComponentException("No IGoapController found on GameObject of GoapBehaviour");

            this.goap = new Goap(controller);

            if (this.configInitializer != null)
                this.configInitializer.InitConfig(this.Config);

            controller.Initialize(this.goap);

            this.CreateAgentTypes();

            this.isInitialized = true;
        }

        private void CreateAgentTypes()
        {
            var agentTypeFactory = new AgentTypeFactory(this.Config);

            foreach (var factory in this.agentTypeConfigFactories)
            {
                if (factory == null)
                    continue;

                this.Register(agentTypeFactory.Create(factory.Create()));
            }
        }

        public IGoapEvents Events => this.goap.Events;
        public Dictionary<IAgentType, IAgentTypeJobRunner> AgentTypeRunners => this.goap.AgentTypeRunners;
        public IGoapController Controller => this.goap.Controller;
        public IGraph GetGraph(IAgentType agentType) => this.goap.GetGraph(agentType);
        public bool Knows(IAgentType agentType) => this.goap.Knows(agentType);
        public List<IMonoGoapActionProvider> Agents => this.goap.Agents;
        public IAgentType[] AgentTypes => this.goap.AgentTypes;
        public IGoapConfig Config => this.goap.Config;

        public IAgentType GetAgentType(string id)
        {
            this.Initialize();

            return this.goap.GetAgentType(id);
        }
    }
}
