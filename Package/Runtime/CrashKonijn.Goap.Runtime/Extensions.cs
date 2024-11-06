using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using JetBrains.Annotations;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    public static class Extensions
    {
        public static bool IsNull(this object obj)
        {
            if (obj is not MonoBehaviour mono)
                return obj == null;

            return mono == null;
        }
        
        public static string ToName(this Comparison comparison)
        {
            return comparison switch
            {
                Comparison.SmallerThan => "<",
                Comparison.SmallerThanOrEqual => "<=",
                Comparison.GreaterThan => ">",
                Comparison.GreaterThanOrEqual => ">=",
                _ => throw new NotImplementedException(),
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
                _ => throw new NotImplementedException(),
            };
        }

        public static string ToName(this EffectType type)
        {
            return type switch
            {
                EffectType.Increase => "++",
                EffectType.Decrease => "--",
                _ => throw new NotImplementedException(),
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
            if (string.IsNullOrEmpty(classRef.Name) && string.IsNullOrEmpty(classRef.Id))
            {
                return (ClassRefStatus.Empty, null);
            }

            // Full match
            if (scripts.Any(x => x.Id == classRef.Id && x.Type.Name == classRef.Name))
            {
                return (ClassRefStatus.Full, scripts.First(x => x.Id == classRef.Id && x.Type.Name == classRef.Name));
            }

            // Id Match
            if (scripts.Any(x => x.Id == classRef.Id))
            {
                return (ClassRefStatus.Id, scripts.First(x => x.Id == classRef.Id));
            }

            // Name Match
            if (scripts.Any(x => x.Type.Name == classRef.Name))
            {
                return (ClassRefStatus.Name, scripts.First(x => x.Type.Name == classRef.Name));
            }

            return (ClassRefStatus.None, null);
        }

        public static T GetInstance<T>(this Script script)
            where T : class
        {
            if (script?.Type == null)
                return null;

            var instance = Activator.CreateInstance(script.Type);

            if (instance is TargetKeyBase targetKey)
            {
                targetKey.Name = script.Type.Name;
            }

            if (instance is WorldKeyBase worldKey)
            {
                worldKey.Name = script.Type.Name;
            }

            return instance as T;
        }

        public static string GetFullName([CanBeNull] this Script script)
        {
            return script?.Type.AssemblyQualifiedName ?? "UNDEFINED";
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static IWorldKey[] GetWorldKeys(this IAgentTypeConfig agentTypeConfig)
        {
            return agentTypeConfig.Actions
                .SelectMany((action) =>
                {
                    return action.Conditions
                        .Where(x => x.WorldKey != null)
                        .Select(y => y.WorldKey);
                })
                .Distinct()
                .ToArray();
        }

        public static ITargetKey[] GetTargetKeys(this IAgentTypeConfig agentTypeConfig)
        {
            return agentTypeConfig.Actions
                .Where(x => x.Target != null)
                .Select(x => x.Target)
                .Distinct()
                .ToArray();
        }
    }
}
