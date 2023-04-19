using System.Linq;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes.Validators
{
    public class TargetSensorKeyValidator : IValidator<IGoapSetConfig>
    {
        public void Validate(IGoapSetConfig config, ValidationResults results)
        {
            var missing = config.TargetSensors.Where(x => x.Key == null).ToArray();
            
            if (!missing.Any())
                return;
            
            results.AddError($"TargetSensors without Key: {string.Join(", ", missing.Select(x => x.Name))}");
        }
    }
}