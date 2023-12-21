using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CrashKonijn.Goap.Attributes;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Scriptables;
using UnityEditor;
using UnityEngine;

namespace CrashKonijn.Goap
{
    public class ClassScanner
    {
        public static Scripts GetClasses(string nameSpace, string folder)
        {
            var scripts = GetDerivedClassNames(nameSpace, folder);

            return new Scripts
            {
                goals = scripts.Where(x => typeof(IGoal).IsAssignableFrom(x.Type)).ToArray(),
                actions = scripts.Where(x => typeof(IAction).IsAssignableFrom(x.Type)).ToArray(),
                targetSensors = scripts.Where(x => typeof(ITargetSensor).IsAssignableFrom(x.Type)).ToArray(),
                worldSensors = scripts.Where(x => typeof(IWorldSensor).IsAssignableFrom(x.Type)).ToArray(),
                multiSensors = scripts.Where(x => typeof(IMultiSensor).IsAssignableFrom(x.Type)).ToArray(),
                worldKeys = scripts.Where(x => typeof(IWorldKey).IsAssignableFrom(x.Type)).ToArray(),
                targetKeys = scripts.Where(x => typeof(ITargetKey).IsAssignableFrom(x.Type)).ToArray(),
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
                var script = GetScript(scriptPath, namespaceName);
                
                if (script == null)
                    continue;
                
                classNames.Add(script);
            }

            return classNames;
        }

        public static Script GetScript(string path, string namespaceName)
        {
            // Load the script asset
            var script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
            if (script == null)
                return null;
                
            // Get the type represented by the script
            var scriptType = script.GetClass();
            
            if (scriptType?.Namespace == null)
                return null;

            if (scriptType.IsAbstract)
                return null;

            if (!scriptType.Namespace.ToLower().StartsWith(namespaceName.ToLower()))
                return null;

            var id = scriptType.GetCustomAttribute<GoapIdAttribute>();
            
            return new Script
            {
                Name = scriptType.Name,
                Type = scriptType,
                Path = path,
                Id = id?.Id
            };
        }

        public static GeneratorScriptable GetGenerator(ScriptableObject scriptable)
        {
            var path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(scriptable));
            var generators = GetGenerators();
            
            foreach (var generator in generators)
            {
                if (path.StartsWith(Path.GetDirectoryName(AssetDatabase.GetAssetPath(generator))))
                {
                    return generator;
                }
            }

            return null;
        }

        public static GeneratorScriptable GetGenerator(string nameSpace)
        {
            return GetGenerators().First(x => x.nameSpace == nameSpace);
        }

        public static GeneratorScriptable[] GetGenerators()
        {
            var assets = AssetDatabase.FindAssets($"t:{nameof(GeneratorScriptable)}");
            
            var generators = assets.Select(AssetDatabase.GUIDToAssetPath).Select(AssetDatabase.LoadAssetAtPath<GeneratorScriptable>);

            return generators.ToArray();
        }
    }

    public class Script
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public string Path { get; set; }
        public string Id { get; set; }
    }

    public class Scripts
    {
        public Script[] goals;
        public Script[] actions;
        public Script[] worldSensors;
        public Script[] worldKeys;
        public Script[] targetSensors;
        public Script[] targetKeys;
        public Script[] multiSensors;
    }
}