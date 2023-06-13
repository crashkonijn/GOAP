using System;
using System.Collections.Generic;
using System.Linq;
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

        [Obsolete("'setConfigFactories' is deprecated, please use 'goapSetConfigFactories' instead.   Exact same functionality, name changed to mitigate confusion with the word 'set' which could have many meanings.")]
        [Header("Obsolete: please use 'GoapSetConfigFactories'")]
        public List<GoapSetFactoryBase> setConfigFactories = new();

        public List<GoapSetFactoryBase> goapSetConfigFactories = new();

        private GoapConfig config;
        private bool isInitialized = false;

        private void Awake()
        {
            this.Initialize();
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

        private void Initialize()
        {
            if (this.isInitialized)
                return;
            
            this.config = GoapConfig.Default;
            this.runner = new Classes.Runners.GoapRunner();
            
            if (this.configInitializer != null)
                this.configInitializer.InitConfig(this.config);
            
            this.CreateGoapSets();
            
            this.isInitialized = true;
        }

        private void CreateGoapSets()
        {
#pragma warning disable CS0618
            if (this.setConfigFactories.Any())
            {
                Debug.LogError("setConfigFactory is obsolete. Please move its data to the goapSetConfigFactories using the editor.");
                this.goapSetConfigFactories.AddRange(this.setConfigFactories);
            }
#pragma warning restore CS0618
            
            var goapSetFactory = new GoapSetFactory(this.config);

            this.goapSetConfigFactories.ForEach(factory =>
            {
                this.Register(goapSetFactory.Create(factory.Create()));
            });
        }

        public Graph GetGraph(IGoapSet goapSet) => this.runner.GetGraph(goapSet);
        public bool Knows(IGoapSet goapSet) => this.runner.Knows(goapSet);
        public IMonoAgent[] Agents => this.runner.Agents;

        [Obsolete("'Sets' is deprecated, please use 'GoapSets' instead.   Exact same functionality, name changed to mitigate confusion with the word 'set' which could have many meanings.")]
        public IGoapSet[] Sets => this.runner.Sets;

        public IGoapSet[] GoapSets => this.runner.GoapSets;

        [Obsolete("'GetSet' is deprecated, please use 'GoapSets' instead.   Exact same functionality, name changed to mitigate confusion with the word 'set' which could have many meanings.")]
        public IGoapSet GetSet(string id)
        {
            this.Initialize();
            
            return this.runner.GetSet(id);
        }

        public IGoapSet GetGoapSet(string id)
        {
            this.Initialize();
            
            return this.runner.GetGoapSet(id);
        }
    }
}