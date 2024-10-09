using UnityEngine;

namespace CrashKonijn.Goap.Demos.Complex
{
    public class Compatibility
    {
        // A simple helper method to find a component in the scene.
        // This supports the new Unity 2023.1 API.
        public static T FindObjectOfType<T>() where T : Object
        {
#if UNITY_2023_1_OR_NEWER
            return Object.FindFirstObjectByType<T>();
#else
            return (T) Object.FindObjectOfType(typeof (T), false);
#endif
        }
        
        // A simple helper method to find all components in the scene.
        // This supports the new Unity 2023.1 API.
        public static T[] FindObjectsOfType<T>() where T : Object
        {
#if UNITY_2023_1_OR_NEWER
            return Object.FindObjectsByType<T>(FindObjectsSortMode.None);
#else 
            return (T[]) Object.FindObjectsOfType(typeof (T));
#endif
        }
    }
}