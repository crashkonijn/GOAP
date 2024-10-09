using System.Reflection;
using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using NSubstitute;
using UnityEngine;

namespace CrashKonijn.Goap.UnitTests.Support
{
    public static class Extensions
    {
        public static void CallAwake<T>(this T behaviour)
            where T : MonoBehaviour
        {
            var method = typeof(T)
                .GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic);

            method.Invoke(behaviour, null);
        }

        public static void CallAwake(this IMonoBehaviour behaviour)
        {
            if (behaviour is not MonoBehaviour monoBehaviour)
                return;

            monoBehaviour.CallAwake();
        }

        public static void CallStart<T>(this T behaviour)
            where T : MonoBehaviour
        {
            typeof(T)
                .GetMethod("Start", BindingFlags.Instance | BindingFlags.NonPublic)!
                .Invoke(behaviour, null);
        }

        public static void CallOnEnable<T>(this T behaviour)
            where T : MonoBehaviour
        {
            typeof(T)
                .GetMethod("OnEnable", BindingFlags.Instance | BindingFlags.NonPublic)!
                .Invoke(behaviour, null);
        }

        public static void CallOnDisable<T>(this T behaviour)
            where T : MonoBehaviour
        {
            typeof(T)
                .GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.NonPublic)!
                .Invoke(behaviour, null);
        }

        public static IGoapAgentEvents MockEvents(this IGoapActionProvider actionProvider)
        {
            var events = Substitute.For<IGoapAgentEvents>();
            // Set Events property through reflection
            typeof(GoapActionProvider)
                .GetField("<Events>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(actionProvider, events);

            return events;
        }

        public static ILogger<IMonoGoapActionProvider> MockLogger(this IGoapActionProvider actionProvider)
        {
            var logger = Substitute.For<ILogger<IMonoGoapActionProvider>>();
            // Set Events property through reflection
            typeof(GoapActionProvider)
                .GetField("<Logger>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(actionProvider, logger);

            return logger;
        }

        public static ILocalWorldData MockWorldData(this IGoapActionProvider actionProvider)
        {
            var worldData = Substitute.For<ILocalWorldData>();
            // Set Events property through reflection
            typeof(GoapActionProvider)
                .GetField("<WorldData>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(actionProvider, worldData);

            return worldData;
        }

        public static IAgentEvents MockEvents(this IAgent agent)
        {
            var events = Substitute.For<IAgentEvents>();
            // Set Events property through reflection
            typeof(AgentBehaviour)
                .GetField("<Events>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(agent, events);

            return events;
        }

        public static void InsertAction(this GoapActionProvider provider, IAction action)
        {
            provider.Receiver.InsertAction(action);
        }

        public static void InsertAction(this IActionReceiver agent, IAction action)
        {
            var actionState = new ActionState();
            actionState.SetAction(action, action.GetData());

            // Set Action property through reflection
            typeof(AgentBehaviour)
                .GetField("<ActionState>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(agent, actionState);
        }
    }
}
