using UnityEngine;

namespace CrashKonijn.Agent.Core
{
    public static class Extensions
    {
        public static Vector3? GetValidPosition(this ITarget target)
        {
            if (target == null)
                return null;

            if (!target.IsValid())
                return null;

            return target.Position;
        }
    }
}
