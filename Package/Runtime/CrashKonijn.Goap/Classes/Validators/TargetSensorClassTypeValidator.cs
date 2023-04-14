using System.Linq;
using CrashKonijn.Goap.Configs.Interfaces;

namespace CrashKonijn.Goap.Classes.Validators
{
    public class TargetSensorClassTypeValidator : IValidator<IGoapSetConfig>
    {
        public void Validate(IGoapSetConfig config, ValidationResults results)
        {
            var empty = config.TargetSensors.Where(x => string.IsNullOrEmpty(x.ClassType)).ToArray();
            
            if (!empty.Any())
                return;
            
            results.AddError($"Target Sensors without ClassType: {string.Join(", ", empty.Select(x => x.Name))}");
        }
    }
}