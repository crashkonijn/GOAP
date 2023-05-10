using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Behaviours
{
    public class AgentEvents : IAgentEvents
    {
        public event ActionDelegate OnActionStart;
        public void ActionStart(IActionBase action)
        {
            this.OnActionStart?.Invoke(action);
        }
        
        public event ActionDelegate OnActionStop;
        public void ActionStop(IActionBase action)
        {
            this.OnActionStop?.Invoke(action);
        }
        
        public event GoalDelegate OnGoalStart;
        public void GoalStart(IGoalBase goal)
        {
            this.OnGoalStart?.Invoke(goal);
        }

        public event GoalDelegate OnGoalCompleted;
        public void GoalCompleted(IGoalBase goal)
        {
            this.OnGoalCompleted?.Invoke(goal);
        }

        public event GoalDelegate OnNoActionFound;
        public void NoActionFound(IGoalBase goal)
        {
            this.OnNoActionFound?.Invoke(goal);
        }
    }
}