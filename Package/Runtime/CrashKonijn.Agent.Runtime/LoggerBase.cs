using System;
using System.Collections.Generic;
using CrashKonijn.Agent.Core;
using UnityEngine;

namespace CrashKonijn.Agent.Runtime
{
    public abstract class LoggerBase<TObj> : ILogger<TObj>
    {
        protected ILoggerConfig config;
        protected TObj source;
        protected abstract string Name { get; }
        public List<string> Logs { get; } = new();

        public void Initialize(ILoggerConfig config, TObj source)
        {
            this.config = config;
            this.source = source;

            this.UnregisterEvents();
            this.RegisterEvents();
        }

        public void Log(string message)
        {
#if UNITY_EDITOR
            this.Handle(message, DebugSeverity.Log);
#endif
        }

        public void Warning(string message)
        {
#if UNITY_EDITOR
            this.Handle(message, DebugSeverity.Warning);
#endif
        }

        public void Error(string message)
        {
#if UNITY_EDITOR
            this.Handle(message, DebugSeverity.Error);
#endif
        }

        public bool ShouldLog()
        {
#if UNITY_EDITOR
            return this.config.DebugMode != DebugMode.None;
#endif

            return false;
        }

        private string FormatLog(string message, DebugSeverity severity)
        {
            var formattedTime = DateTime.Now.ToString("HH:mm:ss");

            return $"<color={this.GetColor(severity)}>[{formattedTime}]</color>: {message}";
        }

        private string GetColor(DebugSeverity severity)
        {
            switch (severity)
            {
                case DebugSeverity.Log:
                    return "white";
                case DebugSeverity.Warning:
                    return "yellow";
                case DebugSeverity.Error:
                    return "red";
                default:
                    return "white";
            }
        }

        private string FormatConsole(string message)
        {
            return $"{this.Name}: {message}";
        }

        protected void Handle(string message, DebugSeverity severity)
        {
            switch (this.config.DebugMode)
            {
                case DebugMode.None:
                    break;
                case DebugMode.Log:
                    this.Store(this.FormatLog(message, severity));
                    break;
                case DebugMode.Console:
                    this.Store(this.FormatLog(message, severity));
                    this.AddToConsole(this.FormatConsole(message), severity);
                    break;
            }
        }

        private void AddToConsole(string message, DebugSeverity severity)
        {
            switch (severity)
            {
                case DebugSeverity.Log:
                    Debug.Log(this.FormatConsole(message));
                    break;
                case DebugSeverity.Warning:
                    Debug.LogWarning(this.FormatConsole(message));
                    break;
                case DebugSeverity.Error:
                    Debug.LogError(this.FormatConsole(message));
                    break;
            }
        }

        private void Store(string message)
        {
            if (this.config.MaxLogSize == 0)
            {
                return;
            }

            if (this.Logs.Count >= this.config.MaxLogSize)
            {
                this.Logs.RemoveAt(0);
            }

            this.Logs.Add(message);
        }

        protected abstract void RegisterEvents();
        protected abstract void UnregisterEvents();

        ~LoggerBase()
        {
            this.UnregisterEvents();
        }
    }
}
