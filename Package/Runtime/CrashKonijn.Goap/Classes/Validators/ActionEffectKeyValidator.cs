using System.Linq;
using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Classes.Validators
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