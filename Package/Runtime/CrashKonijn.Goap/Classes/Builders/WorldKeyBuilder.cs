using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Validators;
using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Classes.Builders
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