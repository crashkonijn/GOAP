using System.Linq;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class WorldSensorKeyValidator : IValidator<IAgentTypeConfig>
    {
        public void Validate(IAgentTypeConfig agentTypeConfig, IValidationResults results)
        {
            var missing = agentTypeConfig.WorldSensors.Where(x => x.Key == null).ToArray();

            if (!missing.Any())
                return;

            results.AddError($"WorldSensors without Key: {string.Join(", ", missing.Select(x => x.Name))}");
        }
    }
}
