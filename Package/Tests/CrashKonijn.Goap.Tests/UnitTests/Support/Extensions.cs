using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using NSubstitute;
using UnityEngine;

namespace CrashKonijn.Goap.UnitTests.Support
{
    public static class Extensions
    {
        public static void CallAwake<T>(this T behaviour)
            where T : MonoBehaviour
        {
            typeof(T)
                .GetMethod("Awake", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
                .Invoke(behaviour, null);
        }
        
        public static void CallStart<T>(this T behaviour)
            where T : MonoBehaviour
        {
            typeof(T)
                .GetMethod("Start", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
                .Invoke(behaviour, null);
        }
        
        public static void CallOnEnable<T>(this T behaviour)
            where T : MonoBehaviour
        {
            typeof(T)
                .GetMethod("OnEnable", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
                .Invoke(behaviour, null);
        }
        
        public static void CallOnDisable<T>(this T behaviour)
            where T : MonoBehaviour
        {
            typeof(T)
                .GetMethod("OnDisable", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
                .Invoke(behaviour, null);
        }

        public static IAgentEvents MockEvents(this AgentBehaviour agent)
        {
            var events = Substitute.For<IAgentEvents>();
            // Set Events property through reflection
            typeof(AgentBehaviour)
                .GetField("<Events>k__BackingField", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(agent, events);

            return events;
        }

        public static void InsertAction(this AgentBehaviour agent, IActionBase action)
        {
            // Set Action property through reflection
            typeof(AgentBehaviour)
                .GetField("<CurrentAction>k__BackingField", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(agent, action);
        }
    }
}