using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CrashKonijn.Goap.Attributes;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;
using UnityEditor;
using UnityEngine;

namespace CrashKonijn.Goap.Support.Loaders
{
    public class ClassScanner
    {
        public static Classes GetClasses(string nameSpace, string folder)
        {
            var scripts = ClassScanner.GetDerivedClassNames(nameSpace, folder);

            return new Classes
            {
                goals = scripts.Where(x => typeof(IGoalBase).IsAssignableFrom(x.type)).ToArray(),
                actions = scripts.Where(x => typeof(IActionBase).IsAssignableFrom(x.type)).ToArray(),
                targetSensors = scripts.Where(x => typeof(ITargetSensor).IsAssignableFrom(x.type)).ToArray(),
                worldSensors = scripts.Where(x => typeof(IWorldSensor).IsAssignableFrom(x.type)).ToArray(),
                worldKeys = scripts.Where(x => typeof(ITargetKey).IsAssignableFrom(x.type)).ToArray(),
                targetKeys = scripts.Where(x => typeof(IWorldKey).IsAssignableFrom(x.type)).ToArray(),
            };
        }
        
        public static List<Script> GetDerivedClassNames(string namespaceName, string folder)
        {
            var classNames = new List<Script>();

            // Get all scripts in the /Assets folder
            var scriptGuids = AssetDatabase.FindAssets("t:script", new[] { folder });
            
            foreach (var scriptGuid in scriptGuids)
            {
                var scriptPath = AssetDatabase.GUIDToAssetPath(scriptGuid);
                
                // Load the script asset
                var script = AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath);
                if (script == null)
                    continue;
                
                // Get the type represented by the script
                var scriptType = script.GetClass();
                if (scriptType?.Namespace == null)
                    continue;

                if (scriptType.IsAbstract)
                    continue;

                if (!scriptType.Namespace.ToLower().StartsWith(namespaceName.ToLower()))
                    continue;

                var id = scriptType.GetCustomAttribute<GoapIdAttribute>();
                
                classNames.Add(new Script
                {
                    type = scriptType,
                    path = scriptPath,
                    id = id?.Id
                });
            }

            return classNames;
        }
    }

    public class Script
    {
        public Type type;
        public string path;
        public string id;
    }

    public class Classes
    {
        public Script[] goals;
        public Script[] actions;
        public Script[] worldSensors;
        public Script[] worldKeys;
        public Script[] targetSensors;
        public Script[] targetKeys;
    }
}