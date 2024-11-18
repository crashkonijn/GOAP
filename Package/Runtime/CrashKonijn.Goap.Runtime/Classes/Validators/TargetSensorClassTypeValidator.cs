using System.Linq;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class TargetSensorClassTypeValidator : IValidator<IAgentTypeConfig>
    {
        public void Validate(IAgentTypeConfig agentTypeConfig, IValidationResults results)
        {
            var empty = agentTypeConfig.TargetSensors.Where(x => string.IsNullOrEmpty(x.ClassType)).ToArray();

            if (!empty.Any())
                return;

            results.AddError($"Target Sensors without ClassType: {string.Join(", ", empty.Select(x => x.Name))}");
        }
    }
}
