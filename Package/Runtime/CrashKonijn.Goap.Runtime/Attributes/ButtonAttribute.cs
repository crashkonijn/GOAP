using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    public class ButtonAttribute : PropertyAttribute
    {
        public string MethodName { get; }

        public ButtonAttribute(string methodName)
        {
            this.MethodName = methodName;
        }
    }
}
