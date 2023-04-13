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
    }
}