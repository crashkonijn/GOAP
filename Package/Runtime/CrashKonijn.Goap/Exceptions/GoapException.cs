using System;

namespace CrashKonijn.Goap.Exceptions
{
    public class GoapException : Exception
    {
        public GoapException(string message) : base(message)
        {
        }
    }
}