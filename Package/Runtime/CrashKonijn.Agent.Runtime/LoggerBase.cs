using System;
using System.Collections.Generic;
using System.Text;
using CrashKonijn.Agent.Core;

namespace CrashKonijn.Agent.Runtime
{
    public abstract class LoggerBase<TObj> : ILogger<TObj>
    {
        protected ILoggerConfig config;
        protected TObj source;
        protected abstract string Name { get; }
        public List<IRawLog> Logs { get; } = new();

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
            this.Handle((builder) => builder.Append(message), DebugSeverity.Log);
#endif
        }

        public void Log(Action<StringBuilder> callback)
        {
#if UNITY_EDITOR
            this.Handle(callback, DebugSeverity.Log);
#endif
        }

        public void Warning(string message)
        {
#if UNITY_EDITOR
            this.Handle((builder) => builder.Append(message), DebugSeverity.Warning);
#endif
        }

        public void Warning(Action<StringBuilder> callback)
        {
#if UNITY_EDITOR
            this.Handle(callback, DebugSeverity.Warning);
#endif
        }

        public void Error(string message)
        {
#if UNITY_EDITOR
            this.Handle((builder) => builder.Append(message), DebugSeverity.Error);
#endif
        }

        public void Error(Action<StringBuilder> callback)
        {
#if UNITY_EDITOR
            this.Handle(callback, DebugSeverity.Error);
#endif
        }

        private string FormatLog(string message, DebugSeverity severity)
        {
            var formattedTime = System.DateTime.Now.ToString("HH:mm:ss");
            
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
        
        private string FormatConsole(IRawLog log)
        {
            return $"{this.Name}: {log.Message}";
        }

        protected void Handle(Action<StringBuilder> message, DebugSeverity severity)
        {
            switch (this.config.DebugMode)
            {
                case DebugMode.None:
                    break;
                case DebugMode.Log:
                    this.Store(message, severity);
                    break;
                case DebugMode.Console:
                    var log = this.Store(message, severity);
                    
                    if (log == null)
                        return;
                    
                    this.AddToConsole(log);
                    break;
            }
        }
        
        private void AddToConsole(IRawLog log)
        {
            switch (log.Severity)
            {
                case DebugSeverity.Log:
                    UnityEngine.Debug.Log(this.FormatConsole(log));
                    break;
                case DebugSeverity.Warning:
                    UnityEngine.Debug.LogWarning(this.FormatConsole(log));
                    break;
                case DebugSeverity.Error:
                    UnityEngine.Debug.LogError(this.FormatConsole(log));
                    break;
            }
        }
        
        private IRawLog Store(Action<StringBuilder> message, DebugSeverity severity)
        {
            if (this.config.MaxLogSize == 0)
            {
                return null;
            }

            IRawLog log = null;
            
            if (this.Logs.Count >= this.config.MaxLogSize)
            {
                log = this.Logs[0];
                this.Logs.RemoveAt(0);
            }
            
            log ??= new RawLog();
            
            log.Callback = message;
            log.Severity = severity;
            
            this.Logs.Add(log);

            return log;
        }

        protected abstract void RegisterEvents();
        protected abstract void UnregisterEvents();
        
        ~LoggerBase()
        {
            this.UnregisterEvents();
        }

        private class RawLog : IRawLog
        {
            private static StringBuilder builder = new();
            
            private Action<StringBuilder> callback;
            public Action<StringBuilder> Callback
            {
                get => this.callback;
                set
                {
                    this.callback = value;
                    this.message = null;
                }
            }
            
            public DebugSeverity Severity { get; set; }

            private string message = null;
            public string Message
            {
                get
                {
                    if (this.message != null)
                        return this.message;

                    builder.Clear();
                    this.callback(builder);
                    this.message = builder.ToString();

                    return this.message;
                }
            }

            public override string ToString()
            {
                return this.Message;
            }
        }
    }
}