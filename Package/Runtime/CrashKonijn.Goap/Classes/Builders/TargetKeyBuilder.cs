using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Validators;
using CrashKonijn.Goap.Configs.Interfaces;

namespace CrashKonijn.Goap.Classes.Builders
{
    public class TargetKeyBuilder : KeyBuilderBase<ITargetKey>
    {
        protected override void InjectData(ITargetKey key)
        {
            if (key is TargetKeyBase targetKey)
                targetKey.Name = key.GetType().GetGenericTypeName();
        }
    }
}