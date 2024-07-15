namespace CrashKonijn.Goap.Core
{
    public interface IEffect
    {
        public IWorldKey WorldKey { get; }
        public bool Increase { get; }
    }
}