using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.GOAP.Demos.TurnBased.Behaviours
{
    public class TurnGoapConfigInitializer : GoapConfigInitializerBase
    {
        public override void InitConfig(IGoapConfig config)
        {
            config.GoapInjector = this.GetComponent<InjectBehaviour>();
        }
    }
}
