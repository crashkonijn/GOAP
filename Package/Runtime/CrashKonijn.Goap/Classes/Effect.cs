using CrashKonijn.Goap.Configs.Interfaces;

namespace CrashKonijn.Goap.Classes
{
    public interface IEffect : LamosInteractive.Goap.Interfaces.IEffect
    {
        public IWorldKey WorldKey { get; }
        public bool Positive { get; }
    }
    
    public class Effect : IEffect
    {
        public IWorldKey WorldKey { get; set; }
        public bool Positive { get; set; }
    }
}