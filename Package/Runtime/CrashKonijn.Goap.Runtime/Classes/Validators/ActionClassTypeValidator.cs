using System.Linq;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class ActionClassTypeValidator : IValidator<IAgentTypeConfig>
    {
        public void Validate(IAgentTypeConfig agentTypeConfig, IValidationResults results)
        {
            var empty = agentTypeConfig.Actions.Where(x => string.IsNullOrEmpty(x.ClassType)).ToArray();

            if (!empty.Any())
                return;

            results.AddError($"Actions without ClassType: {string.Join(", ", empty.Select(x => x.Name))}");
        }
    }
}
