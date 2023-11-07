using System.Linq;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes.Validators
{
    public class TargetSensorKeyValidator : IValidator<IAgentTypeConfig>
    {
        public void Validate(IAgentTypeConfig agentTypeConfig, ValidationResults results)
        {
            var missing = agentTypeConfig.TargetSensors.Where(x => x.Key == null).ToArray();
            
            if (!missing.Any())
                return;
            
            results.AddError($"TargetSensors without Key: {string.Join(", ", missing.Select(x => x.Name))}");
        }
    }
}