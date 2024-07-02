using System.Collections.Generic;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class ManualController : IGoapController
    {
        private IGoap goap;

        public void Initialize(IGoap goap)
        {
            this.goap = goap;
            this.goap.Events.OnAgentResolve += this.OnAgentResolve;
        }

        public void Disable()
        {
            this.goap.Events.OnAgentResolve -= this.OnAgentResolve;
        }

        public void OnUpdate()
        {
            
        }

        public void OnLateUpdate()
        {
            
        }

        private void OnAgentResolve(IGoapAgent agent)
        {
            var runner = this.GetRunner(agent);
            
            runner.Run(new HashSet<IMonoGoapActionProvider>() { agent as IMonoGoapActionProvider });
            runner.Complete();
        }

        private IAgentTypeJobRunner GetRunner(IGoapAgent agent)
        {
            return this.goap.AgentTypeRunners[agent.AgentType];
        }
    }
}