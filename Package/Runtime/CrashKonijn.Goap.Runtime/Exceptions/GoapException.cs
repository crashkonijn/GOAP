using System;

namespace CrashKonijn.Goap.Runtime
{
    public class GoapException : Exception
    {
        public GoapException(string message) : base(message)
        {
        }
    }
}