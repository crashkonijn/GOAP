using System.Linq;
using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Classes.Validators
{
    public class GoalClassTypeValidator : IValidator<IAgentTypeConfig>
    {
        public void Validate(IAgentTypeConfig agentTypeConfig, IValidationResults results)
        {
            var empty = agentTypeConfig.Goals.Where(x => string.IsNullOrEmpty(x.ClassType)).ToArray();
            
            if (!empty.Any())
                return;
            
            results.AddError($"Goals without ClassType: {string.Join(", ", empty.Select(x => x.Name))}");
        }
    }
}