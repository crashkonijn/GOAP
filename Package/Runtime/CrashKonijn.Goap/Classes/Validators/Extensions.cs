using System;
using System.Linq;
using CrashKonijn.Goap.Configs.Interfaces;

namespace CrashKonijn.Goap.Classes.Validators
{
    public static class Extensions
    {
        public static IWorldKey[] GetWorldKeys(this IGoapSetConfig goapSetConfig)
        {
            return goapSetConfig.Actions
                .SelectMany((action) =>
                {
                    return action.Conditions
                        .Where(x => x.WorldKey != null)
                        .Select(y => y.WorldKey);
                })
                .Distinct()
                .ToArray();
        }
        
        public static ITargetKey[] GetTargetKeys(this IGoapSetConfig goapSetConfig)
        {
            return goapSetConfig.Actions
                .Where(x => x.Target != null)
                .Select(x => x.Target)
                .Distinct()
                .ToArray();
        }
        
        public static string GetGenericTypeName(this Type type)
        {
            var typeName = type.Name;

            if (type.IsGenericType)
            {
                var genericArguments = type.GetGenericArguments();
                var genericTypeName = typeName.Substring(0, typeName.IndexOf('`'));
                var typeArgumentNames = string.Join(",", genericArguments.Select(a => a.Name));
                typeName = $"{genericTypeName}<{typeArgumentNames}>";
            }

            return typeName;
        }
    }
}