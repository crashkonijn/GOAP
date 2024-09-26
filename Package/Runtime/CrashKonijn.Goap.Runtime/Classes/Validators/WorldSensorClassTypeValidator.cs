using System.Linq;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class WorldSensorClassTypeValidator : IValidator<IAgentTypeConfig>
    {
        public void Validate(IAgentTypeConfig agentTypeConfig, IValidationResults results)
        {
            var empty = agentTypeConfig.WorldSensors.Where(x => string.IsNullOrEmpty(x.ClassType)).ToArray();

            if (!empty.Any())
                return;

            results.AddError($"World Sensors without ClassType: {string.Join(", ", empty.Select(x => x.Name))}");
        }
    }
}
