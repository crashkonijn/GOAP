using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class WorldKeyBuilder : KeyBuilderBase<IWorldKey>
    {
        protected override void InjectData(IWorldKey key)
        {
            if (key is WorldKeyBase worldKey)
                worldKey.Name = key.GetType().GetGenericTypeName();
        }
    }
}