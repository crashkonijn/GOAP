using System.Linq;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes.Validators
{
    public class GoalConditionKeyValidator : IValidator<IAgentTypeConfig>
    {
        public void Validate(IAgentTypeConfig agentTypeConfig, ValidationResults results)
        {
            foreach (var configGoal in agentTypeConfig.Goals)
            {
                var missing = configGoal.Conditions.Where(x => x.WorldKey == null).ToArray();
                
                if (!missing.Any())
                    continue;
                
                results.AddError($"Goal {configGoal.Name} has conditions without WorldKey");
            }
        }
    }
}