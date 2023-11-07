using System.Linq;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes.Validators
{
    public class TargetSensorClassTypeValidator : IValidator<IAgentTypeConfig>
    {
        public void Validate(IAgentTypeConfig agentTypeConfig, ValidationResults results)
        {
            var empty = agentTypeConfig.TargetSensors.Where(x => string.IsNullOrEmpty(x.ClassType)).ToArray();
            
            if (!empty.Any())
                return;
            
            results.AddError($"Target Sensors without ClassType: {string.Join(", ", empty.Select(x => x.Name))}");
        }
    }
}