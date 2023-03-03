using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolver;
using LamosInteractive.Goap.Models;
using Unity.Collections;
using Unity.Mathematics;

namespace CrashKonijn.Goap.Classes.Runners
{
    public class GoapSetJobRunner
    {
        private readonly IGoapSet goapSet;
        private readonly GraphResolver resolver;
        
        private List<JobRunHandle> resolveHandles = new();
        private readonly ExecutableBuilder executableBuilder;
        private readonly PositionBuilder positionBuilder;

        public GoapSetJobRunner(IGoapSet goapSet)
        {
            this.goapSet = goapSet;
            this.resolver = new GraphResolver(goapSet.GetAllNodes().ToArray(), goapSet.GoapConfig.KeyResolver);
            
            this.executableBuilder = this.resolver.GetExecutableBuilder();
            this.positionBuilder = this.resolver.GetPositionBuilder();
        }

        public void Run()
        {
            this.resolveHandles = new();
            
            this.goapSet.SensorRunner.Update();
            
            var globalData = this.goapSet.SensorRunner.SenseGlobal();

            foreach (var agent in this.goapSet.Agents.All())
            {
                this.Run(globalData, agent);
            }
        }

        private void Run(GlobalWorldData globalData, IMonoAgent agent)
        {
            if (agent.CurrentGoal == null)
                return;

            if (agent.CurrentAction != null)
                return;
            
            var localData = this.goapSet.SensorRunner.SenseLocal(globalData, agent);

            this.FillBuilders(localData);
            
            this.resolveHandles.Add(new JobRunHandle(agent)
            {
                Handle = this.resolver.StartResolve(new RunData
                {
                    StartIndex = this.resolver.GetIndex(agent.CurrentGoal),
                    IsExecutable = new NativeArray<bool>(this.executableBuilder.Build(), Allocator.TempJob),
                    Positions = new NativeArray<float3>(this.positionBuilder.Build(), Allocator.TempJob)
                })
            });
        }

        private void FillBuilders(LocalWorldData localData)
        {
            var conditionObserver = this.goapSet.GoapConfig.ConditionObserver;
            conditionObserver.SetWorldData(localData);

            this.executableBuilder.Clear();
            this.positionBuilder.Clear();

            foreach (var node in this.goapSet.GetActions())
            {
                var allMet = node.Conditions.All(x => conditionObserver.IsMet(x));

                var target = localData.GetTarget(node);

                this.executableBuilder.SetExecutable(node, allMet);
                
                if (target is not null) this.positionBuilder.SetPosition(node, target.Position);
            }
        }

        public void Complete()
        {
            foreach (var resolveHandle in this.resolveHandles)
            {
                var result = resolveHandle.Handle.Complete().OfType<IActionBase>().ToList();
                var action = result.FirstOrDefault();
                
                resolveHandle.Agent.SetAction(action, result, resolveHandle.Agent.WorldData.GetTarget(action));
            }
        }

        public void Dispose()
        {
            this.resolver.Dispose();
        }

        private class JobRunHandle
        {
            public IMonoAgent Agent { get; }
            public ResolveHandle Handle { get; set; }
            
            public JobRunHandle(IMonoAgent agent)
            {
                this.Agent = agent;
            }
        }

        public Graph GetGraph() => this.resolver.GetGraph();
    }
}