using System.Linq;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class GoalConditionKeyValidator : IValidator<IAgentTypeConfig>
    {
        public void Validate(IAgentTypeConfig agentTypeConfig, IValidationResults results)
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
