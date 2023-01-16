using System;
using UnityEngine;

namespace CrashKonijn.Goap.Unity.Injectors
{
    public class GoapInjector : MonoBehaviour
    {
        private readonly ServiceCollection services = new();

        private void Awake()
        {
            
        }

        public T Get<T>()
            where T : class
        {
            return this.services.Get<T>();
        }
    }
}