using UnityEngine;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IComponentReference
    {
        [System.Obsolete("'GetComponent<T>' is deprecated, please use 'GetCachedComponent<T>' instead.   Exact same functionality, name changed to better communicate code usage.")]
        T GetComponent<T>()
            where T : MonoBehaviour;

        T GetCachedComponent<T>()
            where T : MonoBehaviour;

        [System.Obsolete("'GetComponentInChildren<T>' is deprecated, please use 'GetCachedComponentInChildren<T>' instead.   Exact same functionality, name changed to better communicate code usage.")]
        T GetComponentInChildren<T>()
            where T : MonoBehaviour;

        T GetCachedComponentInChildren<T>()
            where T : MonoBehaviour;
    }
}