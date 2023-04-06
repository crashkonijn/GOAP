using System.Collections.Generic;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolver.Models;
using UnityEngine;

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
        public List<GoapSetFactoryBase> setConfigFactories = new();

        private GoapConfig config;

        private void Awake()
        {
            this.config = GoapConfig.Default;
            this.runner = new Classes.Runners.GoapRunner();
            
            if (this.configInitializer != null)
                this.configInitializer.InitConfig(this.config);
            
            this.CreateSets();
        }

        public void Register(IGoapSet set) => this.runner.Register(set);

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

        private void CreateSets()
        {
            var setFactory = new GoapSetFactory(this.config);
            
            this.setConfigFactories.ForEach(factory =>
            {
                this.Register(setFactory.Create(factory.Create()));
            });
        }

        public Graph GetGraph(IGoapSet set) => this.runner.GetGraph(set);
        public bool Knows(IGoapSet set) => this.runner.Knows(set);
        public IMonoAgent[] Agents => this.runner.Agents;
        public IGoapSet[] Sets => this.runner.Sets;
    }
}