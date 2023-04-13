using CrashKonijn.Goap.Configs.Interfaces;

namespace CrashKonijn.Goap.Configs
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