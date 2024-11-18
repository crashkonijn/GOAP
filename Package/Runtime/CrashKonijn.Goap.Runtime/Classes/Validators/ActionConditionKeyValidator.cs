using System.Linq;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class ActionConditionKeyValidator : IValidator<IAgentTypeConfig>
    {
        public void Validate(IAgentTypeConfig agentTypeConfig, IValidationResults results)
        {
            foreach (var configAction in agentTypeConfig.Actions)
            {
                var missing = configAction.Conditions.Where(x => x.WorldKey == null).ToArray();

                if (!missing.Any())
                    continue;

                results.AddError($"Action {configAction.Name} has conditions without WorldKey");
            }
        }
    }
}
