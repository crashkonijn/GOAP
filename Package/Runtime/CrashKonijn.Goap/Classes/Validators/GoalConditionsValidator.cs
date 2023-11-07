using System.Linq;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes.Validators
{
    public class GoalConditionsValidator : IValidator<IAgentTypeConfig>
    {
        public void Validate(IAgentTypeConfig agentTypeConfig, ValidationResults results)
        {
            var missing = agentTypeConfig.Goals.Where(x => !x.Conditions.Any()).ToArray();
            
            if (!missing.Any())
                return;
            
            results.AddWarning($"Goals without Conditions: {string.Join(", ", missing.Select(x => x.Name))}");
        }
    }
}