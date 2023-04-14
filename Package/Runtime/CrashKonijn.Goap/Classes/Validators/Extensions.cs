using System;
using System.Linq;
using CrashKonijn.Goap.Configs.Interfaces;

namespace CrashKonijn.Goap.Classes.Validators
{
    public static class Extensions
    {
        public static IWorldKey[] GetWorldKeys(this IGoapSetConfig config)
        {
            return config.Actions
                .SelectMany((action) =>
                {
                    return action.Conditions.Select(y => y.WorldKey);
                })
                .Distinct()
                .ToArray();
        }
        
        public static ITargetKey[] GetTargetKeys(this IGoapSetConfig config)
        {
            return config.Actions.Select(x => x.Target).Distinct().ToArray();
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