using System.Linq;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class ActionEffectsValidator : IValidator<IAgentTypeConfig>
    {
        public void Validate(IAgentTypeConfig agentTypeConfig, IValidationResults results)
        {
            var missing = agentTypeConfig.Actions.Where(x => !x.Effects.Any()).ToArray();

            if (!missing.Any())
                return;

            results.AddWarning($"Actions without Effects: {string.Join(", ", missing.Select(x => x.Name))}");
        }
    }
}
