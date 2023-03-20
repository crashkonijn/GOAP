using CrashKonijn.Goap.Configs;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    public abstract class GoapSetFactoryBase : MonoBehaviour
    {
        public abstract IGoapSetConfig Create();
    }
}