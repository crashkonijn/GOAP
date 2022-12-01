using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Resolvers
{
    public class ClassResolver
    {
        public HashSet<IActionBase> Load(IEnumerable<ActionConfig> list)
        {
            IActionBase action;
            
            return list.Where(x => !string.IsNullOrEmpty(x.actionClass)).Select(x =>
            {
                action = Activator.CreateInstance(Type.GetType(x.actionClass)) as IActionBase;
                action.SetConfig(x);
                return action;
            }).ToHashSet();
        }

        public HashSet<TType> Load<TType, TConfig>(IEnumerable<TConfig> list)
            where TType : class, IHasConfig<TConfig>
            where TConfig : IClassConfig
        {
            TType action;
            
            return list.Where(x => !string.IsNullOrEmpty(x.ClassType)).Select(x =>
            {
                action = Activator.CreateInstance(Type.GetType(x.ClassType)) as TType;
                action?.SetConfig(x);
                return action;
            }).ToHashSet();
        }
        
        public HashSet<T> LoadTypes<T>(IEnumerable<string> list)
        {
            var types = list.Select(Type.GetType);
            var classes = types.Select(Activator.CreateInstance);
            
            return classes.Cast<T>().ToHashSet();
        }
    }
}