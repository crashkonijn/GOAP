using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class GoapConfig : IGoapConfig
    {
        public IConditionObserver ConditionObserver { get; set; }
        public IKeyResolver KeyResolver { get; set; }
        public IGoapInjector GoapInjector { get; set; }

        public static GoapConfig Default => new()
        {
            ConditionObserver = new ConditionObserver(),
            KeyResolver = new KeyResolver(),
            GoapInjector = new DefaultGoapInjector(),
        };
    }
}
