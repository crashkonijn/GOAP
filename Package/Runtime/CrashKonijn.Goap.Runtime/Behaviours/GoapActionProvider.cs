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

        /// <summary>
        /// Request goals of the specified types.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="goalTypes"></param>
        public void RequestGoal(Type[] goalTypes, bool resolve = true)
        {
            this.ValidateSetup();

            var request = this.GetRequestCache();
            request.Goals.Clear();
            foreach (var goalType in goalTypes)
            {
                request.Goals.Add(this.AgentType.ResolveGoal(goalType));
            }

            this.RequestGoal(request, resolve);
        }

        /// <summary>
        /// Request goals of the specified type.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="goalType"></param>
        public void RequestGoal(Type goalType, bool resolve = true)
        {
            this.ValidateSetup();

            var request = this.GetRequestCache();
            request.Goals.Clear();
            request.Goals.Add(this.AgentType.ResolveGoal(goalType));

            this.RequestGoal(request, resolve);
        }

        /// <summary>
        ///     Requests a goal of type TGoal.
        /// </summary>
        /// <typeparam name="TGoal">The type of the goal.</typeparam>
        /// <param name="resolve">Whether to resolve the action after requesting the goal. Defaults to true.</param>
        public void RequestGoal<TGoal>(bool resolve = true)
            where TGoal : IGoal
        {
            this.RequestGoal(typeof(TGoal), resolve);
        }

        /// <summary>
        ///     Requests two goals of types TGoal1 and TGoal2.
        /// </summary>
        /// <typeparam name="TGoal1">The type of the first goal.</typeparam>
        /// <typeparam name="TGoal2">The type of the second goal.</typeparam>
        /// <param name="resolve">Whether to resolve the action after requesting the goals. Defaults to true.</param>
        public void RequestGoal<TGoal1, TGoal2>(bool resolve = true)
            where TGoal1 : IGoal
            where TGoal2 : IGoal
        {
            this.RequestGoal(new []
            {
                typeof(TGoal1),
                typeof(TGoal2)
            }, resolve);
        }

        /// <summary>
        ///     Requests three goals of types TGoal1, TGoal2, and TGoal3.
        /// </summary>
        /// <typeparam name="TGoal1">The type of the first goal.</typeparam>
        /// <typeparam name="TGoal2">The type of the second goal.</typeparam>
        /// <typeparam name="TGoal3">The type of the third goal.</typeparam>
        /// <param name="resolve">Whether to resolve the action after requesting the goals. Defaults to true.</param>
        public void RequestGoal<TGoal1, TGoal2, TGoal3>(bool resolve = true)
            where TGoal1 : IGoal
            where TGoal2 : IGoal
            where TGoal3 : IGoal
        {
            this.RequestGoal(new []
            {
                typeof(TGoal1),
                typeof(TGoal2),
                typeof(TGoal3)
            }, resolve);
        }

        /// <summary>
        ///     Requests four goals of types TGoal1, TGoal2, TGoal3, and TGoal4.
        /// </summary>
        /// <typeparam name="TGoal1">The type of the first goal.</typeparam>
        /// <typeparam name="TGoal2">The type of the second goal.</typeparam>
        /// <typeparam name="TGoal3">The type of the third goal.</typeparam>
        /// <typeparam name="TGoal4">The type of the fourth goal.</typeparam>
        /// <param name="resolve">Whether to resolve the action after requesting the goals. Defaults to true.</param>
        public void RequestGoal<TGoal1, TGoal2, TGoal3, TGoal4>(bool resolve = true)
            where TGoal1 : IGoal
            where TGoal2 : IGoal
            where TGoal3 : IGoal
            where TGoal4 : IGoal
        {
            this.RequestGoal(new []
            {
                typeof(TGoal1),
                typeof(TGoal2),
                typeof(TGoal3),
                typeof(TGoal4)
            }, resolve);
        }

        /// <summary>
        ///     Requests five goals of types TGoal1, TGoal2, TGoal3, TGoal4, and TGoal5.
        /// </summary>
        /// <typeparam name="TGoal1">The type of the first goal.</typeparam>
        /// <typeparam name="TGoal2">The type of the second goal.</typeparam>
        /// <typeparam name="TGoal3">The type of the third goal.</typeparam>
        /// <typeparam name="TGoal4">The type of the fourth goal.</typeparam>
        /// <typeparam name="TGoal5">The type of the fifth goal.</typeparam>
        /// <param name="resolve">Whether to resolve the action after requesting the goals. Defaults to true.</param>
        public void RequestGoal<TGoal1, TGoal2, TGoal3, TGoal4, TGoal5>(bool resolve = true)
            where TGoal1 : IGoal
            where TGoal2 : IGoal
            where TGoal3 : IGoal
            where TGoal4 : IGoal
            where TGoal5 : IGoal
        {
            this.RequestGoal(new []
            {
                typeof(TGoal1),
                typeof(TGoal2),
                typeof(TGoal3),
                typeof(TGoal4),
                typeof(TGoal5)
            }, resolve);
        }

        /// <summary>
        ///     Requests a specific goal.
        /// </summary>
        /// <param name="goal">The goal to request.</param>
        /// <param name="resolve">Whether to resolve the action after requesting the goal.</param>
        public void RequestGoal(IGoal goal, bool resolve)
        {
            this.ValidateSetup();

            var request = this.GetRequestCache();
            request.Goals.Clear();

            request.Goals.Add(goal);

            this.RequestGoal(request, resolve);
        }

        /// <summary>
        ///     Requests a goal based on the provided goal request. This will allow you to request multiple goals at once.
        /// </summary>
        /// <param name="request">The goal request.</param>
        /// <param name="resolve">Whether to resolve the action after requesting the goal.</param>
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

        /// <summary>
        ///     Sets the action based on the provided goal result. This method is called by the resolver.
        /// </summary>
        /// <param name="result">The goal result.</param>
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

        /// <summary>
        ///     Will try and resolve for a new action based on the current goal request.
        /// </summary>
        public override void ResolveAction()
        {
            this.ValidateSetup();

            this.Events.Resolve();
            this.Receiver.Timers.Resolve.Touch();
        }

        /// <summary>
        ///     Clears the current goal.
        /// </summary>
        public void ClearGoal()
        {
            this.CurrentPlan = null;
        }

        /// <summary>
        ///     Sets the distance multiplier for the agent. This is used by the resolver to calculate the cost of distance between
        ///     actions.
        /// </summary>
        /// <param name="multiplier">The distance multiplier.</param>
        public void SetDistanceMultiplier(float multiplier)
        {
            if (multiplier < 0f)
                throw new GoapException("The distance multiplier must be >= 0.");

            this.DistanceMultiplier = multiplier;
        }

        /// <summary>
        ///     Sets the distance multiplier for the agent based on speed, based on the assumption that speed is in units per
        ///     second and cost is similar to a second.
        ///     This is used by the resolver to calculate the cost of distance between actions.
        /// </summary>
        /// <param name="multiplier">The distance multiplier.</param>
        public void SetDistanceMultiplierSpeed(float speed)
        {
            if (speed <= 0f)
                throw new GoapException("The speed value must be greater than 0. To disable the distance multiplier, use SetDistanceMultiplier(0f).");

            this.DistanceMultiplier = 1f / speed;
        }

        private void ValidateSetup()
        {
            if (this.AgentType == null)
                throw new GoapException($"There is no AgentType assigned to the agent '{this.name}'! Please assign one in the inspector or through code in the Awake method.");

            if (this.Receiver == null)
                throw new GoapException($"There is no ActionReceiver assigned to the agent '{this.name}'! You're probably missing the ActionProvider on the AgentBehaviour.");
        }

        /// <summary>
        ///     Gets the actions of the specified type.
        /// </summary>
        /// <typeparam name="TAction">The type of the actions.</typeparam>
        /// <returns>A list of actions of the specified type.</returns>
        public List<TAction> GetActions<TAction>() where TAction : IGoapAction => this.AgentType.GetActions<TAction>();

        public void Disable<TAction>(IActionDisabler disabler) where TAction : IGoapAction
        {
            foreach (var action in this.GetActions<TAction>())
            {
                this.Disable(action, disabler);
            }
        }
        
        public void Enable<TAction>() where TAction : IGoapAction
        {
            foreach (var action in this.GetActions<TAction>())
            {
                this.Enable(action);
            }
        }

        private void OnDestroy()
        {
            this.Logger.Dispose();
        }

        #region Obsolete Methods

        [Obsolete("Use CurrentPlan.Goal instead")]
        public object CurrentGoal { get; set; }

        [Obsolete("Use AgentType instead")]
        public object GoapSet { get; set; }

        #endregion
    }
}
