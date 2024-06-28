using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap
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