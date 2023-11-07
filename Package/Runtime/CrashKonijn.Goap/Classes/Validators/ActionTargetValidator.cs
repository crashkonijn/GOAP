using System.Linq;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes.Validators
{
    public class ActionTargetValidator : IValidator<IAgentTypeConfig>
    {
        public void Validate(IAgentTypeConfig agentTypeConfig, ValidationResults results)
        {
            var missing = agentTypeConfig.Actions.Where(x => x.Target == null).ToArray();
            
            if (!missing.Any())
                return;
            
            results.AddWarning($"Actions without Target: {string.Join(", ", missing.Select(x => x.Name))}");
        }
    }
}