using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Resolver
{
    public interface ICostBuilder
    {
        ICostBuilder SetCost(IConnectable action, float cost);
        float[] Build();
        void Clear();
    }
}