using System.Linq;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class ActionEffectKeyValidator : IValidator<IAgentTypeConfig>
    {
        public void Validate(IAgentTypeConfig agentTypeConfig, IValidationResults results)
        {
            foreach (var configAction in agentTypeConfig.Actions)
            {
                var missing = configAction.Effects.Where(x => x.WorldKey == null).ToArray();

                if (!missing.Any())
                    continue;

                results.AddError($"Action {configAction.Name} has effects without WorldKey");
            }
        }
    }
}
