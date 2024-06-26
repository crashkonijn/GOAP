using System;

namespace CrashKonijn.Agent
{
    public class AgentException : Exception
    {
        public AgentException(string message) : base(message)
        {
        }
    }
}