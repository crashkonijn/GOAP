using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Resolver.Interfaces
{
    public interface ICostBuilder
    {
        ICostBuilder SetCost(IConnectable action, float cost);
        float[] Build();
        void Clear();
    }
}