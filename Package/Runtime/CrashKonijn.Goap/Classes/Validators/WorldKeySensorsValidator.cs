using System.Linq;
using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Classes.Validators
{
    public class WorldKeySensorsValidator : IValidator<IAgentTypeConfig>
    {
        public void Validate(IAgentTypeConfig agentTypeConfig, IValidationResults results)
        {
            var required = agentTypeConfig.GetWorldKeys();
            var provided = agentTypeConfig.WorldSensors
                .Where(x => x.Key != null)
                .Select(x => x.Key)
                .ToArray();
            
            var missing = required.Except(provided).ToHashSet();

            if (!missing.Any())
                return;
            
            results.AddWarning($"WorldKeys without sensors: {string.Join(", ", missing.Select(x => x.Name))}");
        }
    }
}