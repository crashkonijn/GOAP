using System;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    public static class Extensions
    {
        public static T Add<T>(this VisualElement parent, T child, Action<T> callback) where T : VisualElement
        {
            parent.Add(child);

            callback?.Invoke(child);
            
            return child;
        }
    }
}