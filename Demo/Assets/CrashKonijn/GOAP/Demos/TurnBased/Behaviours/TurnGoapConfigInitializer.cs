using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;

namespace CrashKonijn.GOAP.Demos.TurnBased.Behaviours
{
    public class TurnGoapConfigInitializer : GoapConfigInitializerBase
    {
        public override void InitConfig(GoapConfig config)
        {
            config.GoapInjector = this.GetComponent<InjectBehaviour>();
        }
    }
}