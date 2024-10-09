using System.Linq;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class ActionTargetValidator : IValidator<IAgentTypeConfig>
    {
        public void Validate(IAgentTypeConfig agentTypeConfig, IValidationResults results)
        {
            var missing = agentTypeConfig.Actions.Where(x => x.RequiresTarget && x.Target == null).ToArray();

            if (!missing.Any())
                return;

            results.AddWarning($"Actions without Target: {string.Join(", ", missing.Select(x => x.Name))}");
        }
    }
}
