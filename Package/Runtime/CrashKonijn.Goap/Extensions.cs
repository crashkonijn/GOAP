using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Resolver;
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
    }
}