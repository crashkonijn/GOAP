using System;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Core.Interfaces;
using UnityEngine;

namespace CrashKonijn.Agent
{
    [Serializable]
    public class LoggerConfig : ILoggerConfig
    {
        [field:SerializeField]
        public DebugMode DebugMode { get; set; } = DebugMode.Log;

        [field: SerializeField]
        public int MaxLogSize { get; set; } = 20;
    }
}