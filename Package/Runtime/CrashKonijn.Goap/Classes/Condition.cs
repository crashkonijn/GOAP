using CrashKonijn.Goap.Configs.Interfaces;

namespace CrashKonijn.Goap.Classes
{
    public interface ICondition : LamosInteractive.Goap.Interfaces.ICondition {
        public IWorldKey WorldKey { get; }
        public bool Positive { get; }
    }
    
    public class Condition : ICondition
    {
        public IWorldKey WorldKey { get; set; }
        public bool Positive { get; set; }
    }
}