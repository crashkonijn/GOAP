using System.Collections.Generic;

namespace CrashKonijn.Agent.Core
{
    public interface ILogger<TObj> : ILogger
    {
        void Initialize(ILoggerConfig config, TObj obj);
    }

    public interface ILogger
    {
        List<string> Logs { get; }
        void Log(string message);
        void Warning(string message);
        void Error(string message);
        bool ShouldLog();
    }
}