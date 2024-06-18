using System.Collections.Generic;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Classes
{
    public class Logger : ILogger
    {
        private IMonoAgent agent;
        public IMonoAgent Agent
        {
            get => this.agent;
            set
            {
                this.agent = value;
                this.RegisterEvents();
            }
        }
        
        public List<string> Logs { get; } = new();
        
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
        
        private string FormatConsole(string message)
        {
            return $"{this.Agent.name}: {message}";
        }

        private void Handle(string message, DebugSeverity severity)
        {
            switch (this.agent.DebugMode)
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
                    UnityEngine.Debug.Log(this.FormatConsole(message));
                    break;
                case DebugSeverity.Warning:
                    UnityEngine.Debug.LogWarning(this.FormatConsole(message));
                    break;
                case DebugSeverity.Error:
                    UnityEngine.Debug.LogError(this.FormatConsole(message));
                    break;
            }
        }
        
        private void Store(string message)
        {
            if (this.agent.MaxLogSize == 0)
            {
                return;
            }
            
            if (this.Logs.Count >= this.agent.MaxLogSize)
            {
                this.Logs.RemoveAt(0);
            }
            
            this.Logs.Add(message);
        }

        private void RegisterEvents()
        {
            this.UnregisterEvents();
            
            if (this.Agent == null)
                return;
            
            this.Agent.Events.OnActionStart += this.ActionStart;
            this.Agent.Events.OnActionStop += this.ActionStop;
            this.Agent.Events.OnActionComplete += this.ActionComplete;
            this.Agent.Events.OnNoActionFound += this.NoActionFound;
            this.Agent.Events.OnGoalStart += this.GoalStart;
            this.Agent.Events.OnGoalCompleted += this.GoalCompleted;
        }
        
        private void UnregisterEvents()
        {
            if (this.Agent == null)
                return;
            
            this.Agent.Events.OnActionStart -= this.ActionStart;
            this.Agent.Events.OnActionStop -= this.ActionStop;
            this.Agent.Events.OnActionComplete -= this.ActionComplete;
            this.Agent.Events.OnNoActionFound -= this.NoActionFound;
            this.Agent.Events.OnGoalStart -= this.GoalStart;
            this.Agent.Events.OnGoalCompleted -= this.GoalCompleted;
        }
        
        private void ActionStart(IAction action) => this.Handle($"Action {action?.GetType().Name} started", DebugSeverity.Log);
        private void ActionStop(IAction action) => this.Handle($"Action {action?.GetType().Name} stopped", DebugSeverity.Log);
        private void ActionComplete(IAction action) => this.Handle($"Action {action?.GetType().Name} completed", DebugSeverity.Log);
        private void NoActionFound(IGoal goal) => this.Handle($"No action found for goal {goal?.GetType().Name}", DebugSeverity.Warning);
        private void GoalStart(IGoal goal) => this.Handle($"Goal {goal?.GetType().Name} started", DebugSeverity.Log);
        private void GoalCompleted(IGoal goal) => this.Handle($"Goal {goal?.GetType().Name} completed", DebugSeverity.Log);
        
        ~Logger()
        {
            this.UnregisterEvents();
        }
    }
}