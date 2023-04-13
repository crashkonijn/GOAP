using CrashKonijn.Goap.Classes;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    public abstract class GoapConfigInitializerBase : MonoBehaviour
    {
        public abstract void InitConfig(GoapConfig config);
    }
}