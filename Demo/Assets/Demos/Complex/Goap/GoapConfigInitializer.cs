using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;

namespace Demos.Complex.Goap
{
    public class GoapConfigInitializer : GoapConfigInitializerBase
    {
        public override void InitConfig(GoapConfig config)
        {
            config.GoapInjector = this.GetComponent<GoapInjector>();
        }
    }
}