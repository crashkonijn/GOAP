using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{

    public abstract class ActionBase<TActionData> : ActionBase
        where TActionData : IActionData, new()
    {
        public override IActionData GetData()
        {
            return this.CreateData();
        }

        public virtual TActionData CreateData()
        {
            return new TActionData();
        }

        public override void OnStart(IMonoAgent agent, IActionData data) => this.OnStart(agent, (TActionData) data);
        
        public abstract void OnStart(IMonoAgent agent, TActionData data);

        public override ActionRunState Perform(IMonoAgent agent, IActionData data) => this.Perform(agent, (TActionData) data);

        public abstract ActionRunState Perform(IMonoAgent agent, TActionData data);

        public override void OnEnd(IMonoAgent agent, IActionData data) => this.OnEnd(agent, (TActionData) data);
        
        public abstract void OnEnd(IMonoAgent agent, TActionData data);
    }

    public abstract class ActionBase : ScriptableObject, IActionBase
    {
        [SerializeField]
        private ActionConfig config;
        
        public ActionConfig Config => this.config;
        
        // IAction
        public Guid Guid { get; } = Guid.NewGuid();
        public HashSet<LamosInteractive.Goap.Interfaces.IEffect> Effects => this.config.effects.Cast<LamosInteractive.Goap.Interfaces.IEffect>().ToHashSet();
        public HashSet<LamosInteractive.Goap.Interfaces.ICondition> Conditions => this.config.conditions.Cast<LamosInteractive.Goap.Interfaces.ICondition>().ToHashSet();

        public void SetConfig(ActionConfig config)
        {
            this.config = config;
        }

        public virtual float GetDistanceCost(ITarget currentTarget, ITarget otherTarget)
        {
            return Vector3.Distance(currentTarget.Position, otherTarget.Position);
        }
        
        public virtual int GetCost(IWorldData data)
        {
            return this.config.baseCost;
        }

        public abstract IActionData GetData();
        public abstract ActionRunState Perform(IMonoAgent agent, IActionData data);
        public abstract void OnStart(IMonoAgent agent, IActionData data);
        public abstract void OnEnd(IMonoAgent agent, IActionData data);
    }
}