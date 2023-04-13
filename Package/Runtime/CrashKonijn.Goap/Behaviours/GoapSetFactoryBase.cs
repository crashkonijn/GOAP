using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    public abstract class GoapSetFactoryBase : MonoBehaviour
    {
        public abstract IGoapSetConfig Create();
    }
}