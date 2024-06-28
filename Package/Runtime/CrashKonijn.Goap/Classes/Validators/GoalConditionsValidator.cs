using System.Linq;
using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap
{
    public class GoalConditionsValidator : IValidator<IAgentTypeConfig>
    {
        public void Validate(IAgentTypeConfig agentTypeConfig, IValidationResults results)
        {
            var missing = agentTypeConfig.Goals.Where(x => !x.Conditions.Any()).ToArray();
            
            if (!missing.Any())
                return;
            
            results.AddWarning($"Goals without Conditions: {string.Join(", ", missing.Select(x => x.Name))}");
        }
    }
}