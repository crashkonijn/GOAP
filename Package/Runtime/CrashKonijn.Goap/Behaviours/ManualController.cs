using System.Collections.Generic;
using CrashKonijn.Goap.Core.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    public class ManualController : MonoBehaviour, IGoapController
    {
        private IGoap goap;

        public void Initialize(IGoap goap)
        {
            this.goap = goap;
            this.goap.Events.OnAgentResolve += this.OnAgentResolve;
        }

        private void OnDisable()
        {
            this.goap.Events.OnAgentResolve -= this.OnAgentResolve;
        }

        public void OnUpdate()
        {
            
        }

        public void OnLateUpdate()
        {
            
        }

        private void OnAgentResolve(IAgent agent)
        {
            var runner = this.GetRunner(agent);
            
            runner.Run(new HashSet<IMonoAgent>() { agent as IMonoAgent });
            runner.Complete();
        }

        private IAgentTypeJobRunner GetRunner(IAgent agent)
        {
            return this.goap.AgentTypeRunners[agent.AgentType];
        }
    }
}