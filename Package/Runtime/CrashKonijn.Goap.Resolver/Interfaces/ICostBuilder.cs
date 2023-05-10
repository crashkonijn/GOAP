namespace CrashKonijn.Goap.Resolver.Interfaces
{
    public interface ICostBuilder
    {
        CostBuilder SetCost(IAction action, float cost);
        float[] Build();
        void Clear();
    }
}