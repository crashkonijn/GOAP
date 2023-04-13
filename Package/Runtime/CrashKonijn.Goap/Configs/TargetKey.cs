using CrashKonijn.Goap.Configs.Interfaces;

namespace CrashKonijn.Goap.Configs
{
    public class TargetKey : ITargetKey
    {
        public TargetKey(string name)
        {
            this.Name = name;
        }
        
        public string Name { get; }
    }
}