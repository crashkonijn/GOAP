using CrashKonijn.Goap.Core.Enums;

namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface ILoggerConfig
    {
        public DebugMode DebugMode { get; }
        public int MaxLogSize { get; }
    }
}