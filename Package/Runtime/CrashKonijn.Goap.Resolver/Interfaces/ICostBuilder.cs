namespace CrashKonijn.Goap.Resolver.Interfaces
{
    public interface ICostBuilder
    {
        ICostBuilder SetCost(IAction action, float cost);
        float[] Build();
        void Clear();
    }
}