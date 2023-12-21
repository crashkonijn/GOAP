namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IEffect
    {
        public IWorldKey WorldKey { get; }
        public bool Increase { get; }
    }
}