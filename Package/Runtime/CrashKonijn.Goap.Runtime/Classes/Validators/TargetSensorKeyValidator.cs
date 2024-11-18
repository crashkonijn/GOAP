using System.Linq;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class TargetSensorKeyValidator : IValidator<IAgentTypeConfig>
    {
        public void Validate(IAgentTypeConfig agentTypeConfig, IValidationResults results)
        {
            var missing = agentTypeConfig.TargetSensors.Where(x => x.Key == null).ToArray();

            if (!missing.Any())
                return;

            results.AddError($"TargetSensors without Key: {string.Join(", ", missing.Select(x => x.Name))}");
        }
    }
}
