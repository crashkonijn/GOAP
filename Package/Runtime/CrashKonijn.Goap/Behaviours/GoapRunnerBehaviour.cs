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
        
        public List<GoapSetFactoryBase> setConfigFactories = new();

        private void Awake()
        {
            this.runner = new Classes.Runners.GoapRunner();
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
            var setFactory = new GoapSetFactory(GoapConfig.Default);
            
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