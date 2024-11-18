using System;
using CrashKonijn.Agent.Core;
using UnityEngine;

namespace CrashKonijn.Agent.Runtime
{
    [Serializable]
    public class LoggerConfig : ILoggerConfig
    {
        [field: SerializeField]
        public DebugMode DebugMode { get; set; } = DebugMode.Log;

        [field: SerializeField]
        public int MaxLogSize { get; set; } = 20;
    }
}
