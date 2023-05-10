using CrashKonijn.Goap.Classes.Injectors;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Observers;
using CrashKonijn.Goap.Resolvers;

namespace CrashKonijn.Goap.Classes
{
    public class GoapConfig : IGoapConfig
    {
        public IConditionObserver ConditionObserver { get; set; }
        public IKeyResolver KeyResolver { get; set; }
        public IGoapInjector GoapInjector { get; set; }
        
        public static GoapConfig Default => new GoapConfig
        {
            ConditionObserver = new ConditionObserver(),
            KeyResolver = new KeyResolver(),
            GoapInjector = new GoapInjector()
        };
    }
}