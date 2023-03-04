using System.Linq;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Scriptables;

namespace CrashKonijn.Goap.Editor.Classes
{
    public static class Extensions
    {
        public static IWorldKey[] GetWorldKeys(this IGoapSetConfig config)
        {
            return config.Actions
                .SelectMany((action) =>
                {
                    var conditions = action.Conditions.Select(y => y.WorldKey);
                    var effects = action.Effects.Select(z => z.WorldKey);
                    
                    return conditions.Concat(effects);
                })
                .Distinct()
                .ToArray();
        }
        
        public static ITargetKey[] GetTargetKeys(this IGoapSetConfig config)
        {
            return config.Actions.Select(x => x.Target).Distinct().ToArray();
        }
    }
}