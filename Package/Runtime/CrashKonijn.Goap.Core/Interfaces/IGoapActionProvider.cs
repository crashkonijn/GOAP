using System;
using System.Collections.Generic;
using CrashKonijn.Agent.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Core
{
    public interface IGoapActionProvider : IActionProvider
    {
        float DistanceMultiplier { get; }
        IAgentType AgentType { get; }
        IGoalResult CurrentPlan { get; }
        ILocalWorldData WorldData { get; }
        IGoapAgentEvents Events { get; }
        ILogger<IMonoGoapActionProvider> Logger { get; }
        IGoalRequest GoalRequest { get; }
        Vector3 Position { get; }

        [Obsolete("Use RequestGoal<TGoal> instead.")]
        void SetGoal<TGoal>(bool endAction) where TGoal : IGoal;

        [Obsolete("Use RequestGoal instead.")]
        void SetGoal(IGoal goal, bool endAction);

        void RequestGoal<TGoal>(bool endAction)
            where TGoal : IGoal;

        void RequestGoal<TGoal1, TGoal2>(bool endAction)
            where TGoal1 : IGoal
            where TGoal2 : IGoal;

        void RequestGoal<TGoal1, TGoal2, TGoal3>(bool endAction)
            where TGoal1 : IGoal
            where TGoal2 : IGoal
            where TGoal3 : IGoal;

        void RequestGoal<TGoal1, TGoal2, TGoal3, TGoal4>(bool endAction)
            where TGoal1 : IGoal
            where TGoal2 : IGoal
            where TGoal3 : IGoal
            where TGoal4 : IGoal;

        public void RequestGoal<TGoal1, TGoal2, TGoal3, TGoal4, TGoal5>(bool endAction)
            where TGoal1 : IGoal
            where TGoal2 : IGoal
            where TGoal3 : IGoal
            where TGoal4 : IGoal
            where TGoal5 : IGoal;

        void RequestGoal(IGoal goal, bool endAction);
        void RequestGoal(IGoalRequest request, bool endAction);

        void SetAction(IGoalResult result);
        void ClearGoal();
        void StopAction(bool resolveAction = true);
        void SetDistanceMultiplierSpeed(float speed);
        List<TAction> GetActions<TAction>() where TAction : IGoapAction;

        #region Obsolete

        [Obsolete("Use CurrentPlan.Goal instead")]
        public object CurrentGoal { get; set; }

        [Obsolete("Use AgentType instead")]
        public object GoapSet { get; set; }

        #endregion
    }
}
