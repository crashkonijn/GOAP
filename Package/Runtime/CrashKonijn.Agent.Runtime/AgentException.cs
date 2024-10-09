using System;

namespace CrashKonijn.Agent.Runtime
{
    public class AgentException : Exception
    {
        public AgentException(string message) : base(message)
        {
        }
    }
}