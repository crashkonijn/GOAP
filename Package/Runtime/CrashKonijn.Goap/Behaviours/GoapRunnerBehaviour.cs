using System;
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

        [System.Obsolete("'setConfigFactories' is deprecated, please use 'goapSetConfigFactories' instead.   Exact same functionality, name changed to mitigate confusion with the word 'set' which could have many meanings.")]
        [Header("Obsolete: please use 'GoapSetConfigFactories'")]
        public List<GoapSetFactoryBase> setConfigFactories = new();

        public List<GoapSetFactoryBase> goapSetConfigFactories = new();

        private GoapConfig config;

        private void Awake()
        {
            if(goapSetConfigFactories.Count is 0)
                goapSetConfigFactories = setConfigFactories;
            this.config = GoapConfig.Default;
            this.runner = new Classes.Runners.GoapRunner();
            
            if (this.configInitializer != null)
                this.configInitializer.InitConfig(this.config);
            
            this.CreateGoapSets();
        }

        public void Register(IGoapSet goapSet) => this.runner.Register(goapSet);

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

        [System.Obsolete("'CreateSets()' is deprecated, please use 'CreateGoapSets()' instead.   Exact same functionality, name changed to mitigate confusion with the word 'set' which could have many meanings.")]
        private void CreateSets()
        {
            var setFactory = new GoapSetFactory(this.config);
            
            this.setConfigFactories.ForEach(factory =>
            {
                this.Register(setFactory.Create(factory.Create()));
            });
        }

        private void CreateGoapSets()
        {
            var goapSetFactory = new GoapSetFactory(this.config);

            this.goapSetConfigFactories.ForEach(factory =>
            {
                this.Register(goapSetFactory.Create(factory.Create()));
            });
        }


        public Graph GetGraph(IGoapSet goapSet) => this.runner.GetGraph(goapSet);
        public bool Knows(IGoapSet goapSet) => this.runner.Knows(goapSet);
        public IMonoAgent[] Agents => this.runner.Agents;

        [System.Obsolete("'Sets' is deprecated, please use 'GoapSets' instead.   Exact same functionality, name changed to mitigate confusion with the word 'set' which could have many meanings.")]
        public IGoapSet[] Sets => this.runner.Sets;

        public IGoapSet[] GoapSets => this.runner.GoapSets;

        [System.Obsolete("'GetSet' is deprecated, please use 'GoapSets' instead.   Exact same functionality, name changed to mitigate confusion with the word 'set' which could have many meanings.")]
        public IGoapSet GetSet(string id) => this.runner.GetSet(id);

        public IGoapSet GetGoapSet(string id) => this.runner.GetGoapSet(id);
    }
}