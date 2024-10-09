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

        public void OnUpdate() { }

        public void OnLateUpdate() { }

        private void OnAgentResolve(IGoapActionProvider actionProvider)
        {
            var runner = this.GetRunner(actionProvider);

            runner.Run(new[] { actionProvider as IMonoGoapActionProvider });
            runner.Complete();
        }

        private IAgentTypeJobRunner GetRunner(IGoapActionProvider actionProvider)
        {
            return this.goap.AgentTypeRunners[actionProvider.AgentType];
        }
    }
}
