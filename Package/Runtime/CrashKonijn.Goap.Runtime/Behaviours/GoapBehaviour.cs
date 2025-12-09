using System.Collections.Generic;
using CrashKonijn.Goap.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace CrashKonijn.Goap.Runtime
{
    [DefaultExecutionOrder(-100)]
    public class GoapBehaviour : MonoBehaviour, IGoap
    {
        public GoapConfigInitializerBase configInitializer;

        [FormerlySerializedAs("goapSetConfigFactories")]
        public List<AgentTypeFactoryBase> agentTypeConfigFactories = new();

        private Goap goap;

        private bool isInitialized;

        public float RunTime => this.goap.RunTime;
        public float CompleteTime => this.goap.CompleteTime;
        public int RunCount { get; private set; }

        private void Awake()
        {
            this.Initialize();
        }

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

        public void Register(IAgentType agentType)
        {
            this.goap.Register(agentType);
        }

        public IGoapEvents Events => this.goap.Events;
        public Dictionary<IAgentType, IAgentTypeJobRunner> AgentTypeRunners => this.goap.AgentTypeRunners;
        public IGoapController Controller => this.goap.Controller;

        public IGraph GetGraph(IAgentType agentType)
        {
            return this.goap.GetGraph(agentType);
        }

        public bool Knows(IAgentType agentType)
        {
            return this.goap.Knows(agentType);
        }

        public List<IMonoGoapActionProvider> Agents => this.goap.Agents;
        public IAgentType[] AgentTypes => this.goap.AgentTypes;
        public IGoapConfig Config => this.goap.Config;

        public IAgentType GetAgentType(string id)
        {
            this.Initialize();

            return this.goap.GetAgentType(id);
        }

        public void Register(IAgentTypeConfig agentTypeConfig)
        {
            this.goap.Register(new AgentTypeFactory(this.Config).Create(agentTypeConfig));
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

                factory.Construct(this.Config);
                this.Config.GoapInjector.Inject(factory);

                this.Register(agentTypeFactory.Create(factory.Create()));
            }
        }
    }
}