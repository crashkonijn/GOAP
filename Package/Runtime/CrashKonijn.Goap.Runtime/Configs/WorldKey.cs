using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class WorldKey : IWorldKey
    {
        public WorldKey(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}
