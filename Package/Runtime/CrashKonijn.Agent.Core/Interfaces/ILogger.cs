using System;
using System.Collections.Generic;
using System.Text;

namespace CrashKonijn.Agent.Core
{
    public interface ILogger<TObj> : ILogger
    {
        void Initialize(ILoggerConfig config, TObj obj);
    }

    public interface ILogger
    {
        List<IRawLog> Logs { get; }
        void Log(string message);
        void Log(Action<StringBuilder> callback);
        void Warning(string message);
        void Warning(Action<StringBuilder> callback);
        void Error(string message);
        void Error(Action<StringBuilder> callback);
    }
    
    public interface IRawLog
    {
        Action<StringBuilder> Callback { get; set; }
        DebugSeverity Severity { get; set; }
        string Message { get; }
        string ToString();
    }
}