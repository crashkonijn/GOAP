using System.Collections.Generic;
using CrashKonijn.Goap.Core.Enums;

namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface ILogger
    {
        IMonoAgent Agent { get; set; }
        List<string> Logs { get; }
        void Log(string message);
        void Warning(string message);
        void Error(string message);
    }
}