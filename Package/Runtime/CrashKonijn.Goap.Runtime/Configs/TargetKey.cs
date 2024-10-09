using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
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
