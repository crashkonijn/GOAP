using CrashKonijn.Goap.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    public abstract class GoapConfigInitializerBase : MonoBehaviour
    {
        public abstract void InitConfig(IGoapConfig config);
    }
}