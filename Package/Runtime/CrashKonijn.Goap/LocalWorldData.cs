using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap
{
    public class LocalWorldData : WorldDataBase
    {
        public void Apply(IWorldData globalWorldData)
        {
            foreach (var (key, value) in globalWorldData.States)
            {
                this.SetState(key, value);
            }
            
            foreach (var (key, value) in globalWorldData.Targets)
            {
                this.SetTarget(key, value);
            }
        }
    }
}