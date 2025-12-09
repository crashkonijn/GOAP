#if RABBIT_LOGGER_1
using CrashKonijn.Logger;
using System.Linq;
#else
using System;
using UnityEngine;
#endif
using System.Collections.Generic;
using CrashKonijn.Agent.Core;
using ILoggerConfig = CrashKonijn.Agent.Core.ILoggerConfig;

namespace CrashKonijn.Agent.Runtime
{
#if RABBIT_LOGGER_1
    public abstract class LoggerBase<TObj> : ILogger<TObj>
        where TObj : class
    {
        protected ILoggerConfig config;
        protected TObj source;
        public abstract string Name { get; }
        public List<string> Logs => this.logger.Logs.Select(x => x.message).ToList();
        private IRabbitLogger logger;

        public void Initialize(ILoggerConfig config, TObj obj)
        {
            this.source = obj;
            this.config = config;

            this.logger = LoggerFactory.Create<TObj>(obj);

            this.UnregisterEvents();
            this.RegisterEvents();
        }

        public void Log(string message) => this.logger.Log(message);
        public void Warning(string message) => this.logger.Warning(message);
        public void Error(string message) => this.logger.Error(message);
        public bool ShouldLog() => this.logger.ShouldLog();

        protected abstract void RegisterEvents();
        protected abstract void UnregisterEvents();

        ~LoggerBase()
        {
            this.UnregisterEvents();
        }

        public void Dispose()
        {
            this.UnregisterEvents();
            this.logger?.Dispose();
        }
    }

#else
    public abstract class LoggerBase<TObj> : ILogger<TObj>
    {
        protected ILoggerConfig config;
        protected TObj source;
        public abstract string Name { get; }
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
#else
            return false;
#endif
        }

        public void Dispose()
        {
            this.UnregisterEvents();
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
                return;

            if (this.Logs.Count >= this.config.MaxLogSize)
                this.Logs.RemoveAt(0);

            this.Logs.Add(message);
        }

        protected abstract void RegisterEvents();
        protected abstract void UnregisterEvents();

        ~LoggerBase()
        {
            this.UnregisterEvents();
        }
    }
#endif
}