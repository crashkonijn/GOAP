using System;
using System.Collections.Generic;
using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    public class GoapActionProvider : ActionProviderBase, IMonoGoapActionProvider
    {
        [field: SerializeField]
        public LoggerConfig LoggerConfig { get; set; } = new();

        [field: SerializeField]
        public AgentTypeBehaviour AgentTypeBehaviour { get; set; }

        [field: SerializeField]
        public float DistanceMultiplier { get; set; } = 1f;

        private IAgentType agentType;

        public IAgentType AgentType
        {
            get => this.agentType;
            set
            {
                if (this.agentType != null)
                    this.agentType.Unregister(this);

                this.agentType = value;
                this.WorldData.SetParent(value.WorldData);
                this.GoalRequest = null;
                this.CurrentPlan = null;

                value.Register(this);

                this.Events.Bind(this, value.Events);
            }
        }

        public IGoalResult CurrentPlan { get; private set; } = new GoalResult();
        public IGoalRequest GoalRequest { get; private set; }
        private IGoalRequest requestCache = new GoalRequest();

        public ILocalWorldData WorldData { get; } = new LocalWorldData();
        public IGoapAgentEvents Events { get; } = new GoapAgentEvents();
        public ILogger<IMonoGoapActionProvider> Logger { get; } = new GoapAgentLogger();

        public Vector3 Position => this.transform.position;

        private void Awake()
        {
            if (this.AgentTypeBehaviour != null)
                this.AgentType = this.AgentTypeBehaviour.AgentType;

            this.Logger.Initialize(this.LoggerConfig, this);
        }

        private void Start()
        {
            if (this.AgentType == null)
                throw new GoapException($"There is no AgentType assigned to the agent '{this.name}'! Please assign one in the inspector or through code in the Awake method.");
        }

        private void OnEnable()
        {
            if (this.AgentType == null)
                return;

            this.AgentType.Register(this);

            if (this.GoalRequest != null)
                this.ResolveAction();
        }

        private void OnDisable()
        {
            if (this.AgentType == null)
                return;

            this.AgentType.Unregister(this);
            this.Events.Unbind();
        }

        [Obsolete("Use RequestGoal<TGoal> instead.")]
        public void SetGoal<TGoal>(bool endAction = false) where TGoal : IGoal => this.RequestGoal<TGoal>(endAction);

        [Obsolete("Use RequestGoal instead.")]
        public void SetGoal(IGoal goal, bool endAction = false) => this.RequestGoal(goal, endAction);

        private IGoalRequest GetRequestCache()
        {
            if (this.requestCache == null)
                this.requestCache = new GoalRequest();

            this.requestCache.Goals.Clear();
            this.requestCache.Key = string.Empty;

            return this.requestCache;
        }

        public void RequestGoal<TGoal>(bool resolve = true)
            where TGoal : IGoal
        {
            this.ValidateSetup();

            var request = this.GetRequestCache();
            request.Goals.Clear();
            request.Goals.Add(this.AgentType.ResolveGoal<TGoal>());

            this.RequestGoal(request, resolve);
        }

        public void RequestGoal<TGoal1, TGoal2>(bool resolve = true)
            where TGoal1 : IGoal
            where TGoal2 : IGoal
        {
            this.ValidateSetup();

            var request = this.GetRequestCache();
            request.Goals.Clear();
            request.Goals.Add(this.AgentType.ResolveGoal<TGoal1>());
            request.Goals.Add(this.AgentType.ResolveGoal<TGoal2>());

            this.RequestGoal(request, resolve);
        }

        public void RequestGoal<TGoal1, TGoal2, TGoal3>(bool resolve = true)
            where TGoal1 : IGoal
            where TGoal2 : IGoal
            where TGoal3 : IGoal
        {
            this.ValidateSetup();

            var request = this.GetRequestCache();
            request.Goals.Clear();
            request.Goals.Add(this.AgentType.ResolveGoal<TGoal1>());
            request.Goals.Add(this.AgentType.ResolveGoal<TGoal2>());
            request.Goals.Add(this.AgentType.ResolveGoal<TGoal3>());

            this.RequestGoal(request, resolve);
        }

        public void RequestGoal<TGoal1, TGoal2, TGoal3, TGoal4>(bool resolve = true)
            where TGoal1 : IGoal
            where TGoal2 : IGoal
            where TGoal3 : IGoal
            where TGoal4 : IGoal
        {
            this.ValidateSetup();

            var request = this.GetRequestCache();
            request.Goals.Clear();
            request.Goals.Add(this.AgentType.ResolveGoal<TGoal1>());
            request.Goals.Add(this.AgentType.ResolveGoal<TGoal2>());
            request.Goals.Add(this.AgentType.ResolveGoal<TGoal3>());
            request.Goals.Add(this.AgentType.ResolveGoal<TGoal4>());

            this.RequestGoal(request, resolve);
        }

        public void RequestGoal<TGoal1, TGoal2, TGoal3, TGoal4, TGoal5>(bool resolve = true)
            where TGoal1 : IGoal
            where TGoal2 : IGoal
            where TGoal3 : IGoal
            where TGoal4 : IGoal
            where TGoal5 : IGoal
        {
            this.ValidateSetup();

            var request = this.GetRequestCache();

            request.Goals.Clear();
            request.Goals.Add(this.AgentType.ResolveGoal<TGoal1>());
            request.Goals.Add(this.AgentType.ResolveGoal<TGoal2>());
            request.Goals.Add(this.AgentType.ResolveGoal<TGoal3>());
            request.Goals.Add(this.AgentType.ResolveGoal<TGoal4>());
            request.Goals.Add(this.AgentType.ResolveGoal<TGoal5>());

            this.RequestGoal(request, resolve);
        }

        public void RequestGoal(IGoal goal, bool resolve)
        {
            this.ValidateSetup();

            var request = this.GetRequestCache();
            request.Goals.Clear();

            request.Goals.Add(goal);

            this.RequestGoal(request, resolve);
        }

        public void RequestGoal(IGoalRequest request, bool resolve = true)
        {
            this.ValidateSetup();

            if (request == null)
                return;

            if (AreEqual(this.GoalRequest?.Goals ?? new List<IGoal>(), request.Goals))
                return;

            this.requestCache = this.GoalRequest;
            this.GoalRequest = request;

            if (this.Receiver == null)
                return;

            if (resolve)
                this.ResolveAction();
        }

        public static bool AreEqual<T>(List<T> array1, List<T> array2)
            where T : class
        {
            if (array1.Count != array2.Count)
                return false;

            for (var i = 0; i < array1.Count; i++)
            {
                if (array1[i] != array2[i])
                    return false;
            }

            return true;
        }

        public void SetAction(IGoalResult result)
        {
            if (result == null)
                return;

            if (this.Logger.ShouldLog())
                this.Logger.Log($"Setting action '{result.Action.GetType().GetGenericTypeName()}' for goal '{result.Goal.GetType().GetGenericTypeName()}'.");

            var currentGoal = this.CurrentPlan?.Goal;

            this.CurrentPlan = result;

            if (this.Receiver == null)
                return;

            this.Receiver.Timers.Goal.Touch();

            if (currentGoal != result.Goal)
                this.Events.GoalStart(result.Goal);

            this.Receiver.SetAction(this, result.Action, this.WorldData.GetTarget(result.Action));
        }

        [Obsolete("Use agent.StopAction() instead")]
        public void StopAction(bool resolveAction = true)
        {
            this.Receiver?.StopAction(resolveAction);
        }

        private IActionReceiver receiver;

        public override IActionReceiver Receiver
        {
            get => this.receiver;
            set
            {
                this.receiver = value;
                this.Events.Bind(value);
            }
        }

        public override void ResolveAction()
        {
            this.ValidateSetup();

            this.Events.Resolve();
            this.Receiver.Timers.Resolve.Touch();
        }

        public void ClearGoal()
        {
            this.CurrentPlan = null;
        }

        public void SetDistanceMultiplierSpeed(float speed)
        {
            this.DistanceMultiplier = 1f / speed;
        }

        private void ValidateSetup()
        {
            if (this.AgentType == null)
                throw new GoapException($"There is no AgentType assigned to the agent '{this.name}'! Please assign one in the inspector or through code in the Awake method.");

            if (this.Receiver == null)
                throw new GoapException($"There is no ActionReceiver assigned to the agent '{this.name}'! You're probably missing the ActionProvider on the AgentBehaviour.");
        }

        public List<TAction> GetActions<TAction>() where TAction : IGoapAction => this.AgentType.GetActions<TAction>();

        #region Obsolete Methods

        [Obsolete("Use CurrentPlan.Goal instead")]
        public object CurrentGoal { get; set; }

        [Obsolete("Use AgentType instead")]
        public object GoapSet { get; set; }

        #endregion
    }
}
