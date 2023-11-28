using System.Collections.Generic;
using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Classes.Controllers
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