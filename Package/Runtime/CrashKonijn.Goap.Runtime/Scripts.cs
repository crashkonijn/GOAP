using System;

namespace CrashKonijn.Goap.Runtime
{
    [Serializable]
    public class Scripts
    {
        public Script[] goals;
        public Script[] actions;
        public Script[] worldSensors;
        public Script[] worldKeys;
        public Script[] targetSensors;
        public Script[] targetKeys;
        public Script[] multiSensors;
    }
}