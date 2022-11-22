using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IGoapRunner
    {
        void Register(GoapSet set);
        void Register(IMonoAgent agent);
        void Unregister(IMonoAgent agent);
    }
}