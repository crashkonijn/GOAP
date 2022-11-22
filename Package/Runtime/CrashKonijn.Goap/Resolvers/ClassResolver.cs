using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Scriptables;

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
        
        public HashSet<IGoalBase> Load(IEnumerable<GoalConfig> list)
        {
            IGoalBase action;
            
            return list.Where(x => !string.IsNullOrEmpty(x.goalClass)).Select(x =>
            {
                action = Activator.CreateInstance(Type.GetType(x.goalClass)) as IGoalBase;
                action.SetConfig(x);
                return action;
            }).ToHashSet();
        }
        
        public HashSet<ITargetSensor> Load(IEnumerable<TargetSensorConfig> list)
        {
            ITargetSensor action;
            
            return list.Where(x => !string.IsNullOrEmpty(x.sensorClass)).Select(x =>
            {
                action = Activator.CreateInstance(Type.GetType(x.sensorClass)) as ITargetSensor;
                action.SetConfig(x);
                return action;
            }).ToHashSet();
        }
        
        public HashSet<IWorldSensor> Load(IEnumerable<WorldSensorConfig> list)
        {
            IWorldSensor action;
            
            return list.Where(x => !string.IsNullOrEmpty(x.sensorClass)).Select(x =>
            {
                action = Activator.CreateInstance(Type.GetType(x.sensorClass)) as IWorldSensor;
                action.SetConfig(x);
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