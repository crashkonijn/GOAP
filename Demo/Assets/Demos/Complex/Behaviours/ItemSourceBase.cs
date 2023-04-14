using Demos.Complex.Interfaces;
using UnityEngine;

namespace Demos.Complex.Behaviours
{
    public class ItemSourceBase<T> : MonoBehaviour, ISource<T>
        where T : IGatherable
    {
    }
}