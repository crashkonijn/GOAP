using System;
using System.Linq;
using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    public class GoapActionProvider : ActionProviderBase, IMonoGoapActionProvider
    {
        [field: SerializeField]
        public LoggerConfig LoggerConfig { get; set; } = new LoggerConfig();
        
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
                this.agentType = value;
                this.WorldData.SetParent(value.WorldData);
                value.Register(this);
                
                this.Events.Bind(this, value.Events);
            }
        }
        public IGoalResult CurrentPlan { get; private set; }
        public IGoalRequest GoalRequest { get; private set; }

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
        }

        private void OnDisable()
        {
            this.StopAction(false);

            if (this.AgentType == null)
                return;
            
            this.AgentType.Unregister(this);
            this.Events.Unbind();
        }

        [Obsolete("Use RequestGoal<TGoal> instead.")]
        public void SetGoal<TGoal>(bool endAction) where TGoal : IGoal => this.RequestGoal<TGoal>(endAction);
        [Obsolete("Use RequestGoal instead.")]
        public void SetGoal(IGoal goal, bool endAction) => this.RequestGoal(goal, endAction);

        public void RequestGoal<TGoal>(bool resolve)
            where TGoal : IGoal
        {
            this.ValidateSetup();
            
            this.RequestGoal(new GoalRequest
            {
                Goals = new IGoal[]
                {
                    this.AgentType.ResolveGoal<TGoal>()
                }
            }, resolve);
        }

        public void RequestGoal<TGoal1, TGoal2>(bool resolve)
            where TGoal1 : IGoal
            where TGoal2 : IGoal
        {
            this.ValidateSetup();

            this.RequestGoal(new GoalRequest
            {
                Goals = new IGoal[]
                {
                    this.AgentType.ResolveGoal<TGoal1>(),
                    this.AgentType.ResolveGoal<TGoal2>()
                }
            }, resolve);
        }
        
        public void RequestGoal<TGoal1, TGoal2, TGoal3>(bool resolve)
            where TGoal1 : IGoal
            where TGoal2 : IGoal
            where TGoal3 : IGoal
        {
            this.ValidateSetup();

            this.RequestGoal(new GoalRequest
            {
                Goals = new IGoal[]
                {
                    this.AgentType.ResolveGoal<TGoal1>(),
                    this.AgentType.ResolveGoal<TGoal2>(),
                    this.AgentType.ResolveGoal<TGoal3>()
                }
            }, resolve);
        }
        
        public void RequestGoal<TGoal1, TGoal2, TGoal3, TGoal4>(bool resolve)
            where TGoal1 : IGoal
            where TGoal2 : IGoal
            where TGoal3 : IGoal
            where TGoal4 : IGoal
        {
            this.ValidateSetup();

            this.RequestGoal(new GoalRequest
            {
                Goals = new IGoal[]
                {
                    this.AgentType.ResolveGoal<TGoal1>(),
                    this.AgentType.ResolveGoal<TGoal2>(),
                    this.AgentType.ResolveGoal<TGoal3>(),
                    this.AgentType.ResolveGoal<TGoal4>()
                }
            }, resolve);
        }
        
        public void RequestGoal<TGoal1, TGoal2, TGoal3, TGoal4, TGoal5>(bool resolve)
            where TGoal1 : IGoal
            where TGoal2 : IGoal
            where TGoal3 : IGoal
            where TGoal4 : IGoal
            where TGoal5 : IGoal
        {
            this.ValidateSetup();

            this.RequestGoal(new GoalRequest
            {
                Goals = new IGoal[]
                {
                    this.AgentType.ResolveGoal<TGoal1>(),
                    this.AgentType.ResolveGoal<TGoal2>(),
                    this.AgentType.ResolveGoal<TGoal3>(),
                    this.AgentType.ResolveGoal<TGoal4>(),
                    this.AgentType.ResolveGoal<TGoal5>()
                }
            }, resolve);
        }

        public void RequestGoal(IGoal goal, bool resolve)
        {
            this.ValidateSetup();

            this.RequestGoal(new GoalRequest
            {
                Goals = new[] { goal }
            }, resolve);
        }

        public void RequestGoal(IGoalRequest request, bool resolve)
        {
            this.ValidateSetup();

            if (request == null)
                return;
            
            if (this.GoalRequest?.Goals.SequenceEqual(request.Goals) ?? false)
                return;
            
            this.GoalRequest = request;

            if (this.Receiver == null)
                return;

            if (resolve)
                this.ResolveAction();
        }

        public void SetAction(IGoalResult result)
        {
            this.Logger.Log($"Setting action '{result.Action.GetType().GetGenericTypeName()}' for goal '{result.Goal.GetType().GetGenericTypeName()}'.");
            
            var currentGoal = this.CurrentPlan?.Goal;
            
            this.CurrentPlan = result;
            
            if (this.Receiver == null)
                return;
            
            this.Receiver.Timers.Goal.Touch();
            
            if (currentGoal != result.Goal)
                this.Events.GoalStart(currentGoal);
            
            this.Receiver.SetAction(this, result.Action, this.WorldData.GetTarget(result.Action));
        }

        public void StopAction(bool resolveAction = true)
        {
            this.Receiver.StopAction(resolveAction);
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
    }
}