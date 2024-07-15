using UnityEngine;

namespace CrashKonijn.Agent.Core
{
    public interface IComponentReference
    {
        [System.Obsolete("'GetComponent<T>' is deprecated, please use 'GetCachedComponent<T>' instead.   Exact same functionality, name changed to better communicate code usage.")]
        T GetComponent<T>();

        T GetCachedComponent<T>();

        [System.Obsolete("'GetComponentInChildren<T>' is deprecated, please use 'GetCachedComponentInChildren<T>' instead.   Exact same functionality, name changed to better communicate code usage.")]
        T GetComponentInChildren<T>();

        T GetCachedComponentInChildren<T>();

        T GetCachedComponentInParent<T>();
    }
}