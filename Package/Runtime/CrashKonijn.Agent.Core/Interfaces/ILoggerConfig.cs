namespace CrashKonijn.Agent.Core
{
    public interface ILoggerConfig
    {
        public DebugMode DebugMode { get; }
        public int MaxLogSize { get; }
    }
}