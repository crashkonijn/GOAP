using CrashKonijn.Goap.Demos.Complex.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Complex.Behaviours
{
    public class ItemSourceBase<T> : MonoBehaviour, ISource<T>
        where T : IGatherable
    {
    }
}