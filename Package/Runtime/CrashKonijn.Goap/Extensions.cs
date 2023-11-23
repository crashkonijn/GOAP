using System;
using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Resolver;
using CrashKonijn.Goap.Scriptables;
using CrashKonijn.Goap.Support.Loaders;
using JetBrains.Annotations;
using UnityEngine;

namespace CrashKonijn.Goap
{
    public static class Extensions
    {
        public static bool IsNull(this IMonoAgent agent)
            => agent is MonoBehaviour mono && mono == null;

        public static string ToName(this Comparison comparison)
        {
            return comparison switch
            {
                Comparison.SmallerThan => "<",
                Comparison.SmallerThanOrEqual => "<=",
                Comparison.GreaterThan => ">",
                Comparison.GreaterThanOrEqual => ">=",
                _ => throw new System.NotImplementedException()
            };
        }
        
        public static Comparison FromName(this string comparison)
        {
            return comparison switch
            {
                "<" => Comparison.SmallerThan,
                "<=" => Comparison.SmallerThanOrEqual,
                ">" => Comparison.GreaterThan,
                ">=" => Comparison.GreaterThanOrEqual,
                _ => throw new System.NotImplementedException()
            };
        }
        
        public static string ToName(this EffectType type)
        {
            return type switch
            {
                EffectType.Increase => "++",
                EffectType.Decrease => "--",
                _ => throw new System.NotImplementedException()
            };
        }
        
        public static ClassRefStatus GetStatus(this IClassRef classRef, Script[] scripts)
        {
            var (status, match) = classRef.GetMatch(scripts);
            
            return status;
        }

        public static Script GetScript(this IClassRef classRef, Script[] scripts)
        {
            var (status, match) = classRef.GetMatch(scripts);
            
            return match;
        }

        public static (ClassRefStatus status, Script script) GetMatch(this IClassRef classRef, Script[] scripts)
        {
            if (classRef.Name == "" && classRef.Id == "")
            {
                return (ClassRefStatus.None, null);
            }
            
            // Full match
            if (scripts.Any(x => x.id == classRef.Id && x.type.Name == classRef.Name))
            {
                return (ClassRefStatus.Full, scripts.First(x => x.id == classRef.Id && x.type.Name == classRef.Name));
            }
            
            // Id Match
            if (scripts.Any(x => x.id == classRef.Id))
            {
                return (ClassRefStatus.Id, scripts.First(x => x.id == classRef.Id));
            }
            
            // Name Match
            if (scripts.Any(x => x.type.Name == classRef.Name))
            {
                return (ClassRefStatus.Name, scripts.First(x => x.type.Name == classRef.Name));
            }
            
            return (ClassRefStatus.None, null);
        }

        public static T GetInstance<T>(this Script script)
            where T : class
        {
            var instance = Activator.CreateInstance(script.type);

            if (instance is TargetKeyBase targetKey)
            {
                targetKey.Name = script.type.Name;
            }

            if (instance is WorldKeyBase worldKey)
            {
                worldKey.Name = script.type.Name;
            }
            
            return instance as T;
        }

        public static string GetFullName([CanBeNull] this Script script)
        {
            return script?.type.AssemblyQualifiedName ?? "UNDEFINED";
        }

        public static GeneratorScriptable GetGenerator(this ScriptableObject scriptable)
        {
            return ClassScanner.GetGenerator(scriptable);
        }
    }
}