using System.Collections.Generic;

namespace CrashKonijn.Goap.Core.Interfaces
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
    }
}