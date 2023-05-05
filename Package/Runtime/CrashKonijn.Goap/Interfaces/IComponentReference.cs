using UnityEngine;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IComponentReference
    {
        T GetComponent<T>()
            where T : MonoBehaviour;
        
        T GetComponentInChildren<T>()
            where T : MonoBehaviour;
    }
}