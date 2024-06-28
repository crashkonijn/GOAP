using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    public abstract class GoapConfigInitializerBase : MonoBehaviour
    {
        public abstract void InitConfig(GoapConfig config);
    }
}