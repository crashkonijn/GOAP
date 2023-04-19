using System.Linq;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes.Validators
{
    public class WorldSensorKeyValidator : IValidator<IGoapSetConfig>
    {
        public void Validate(IGoapSetConfig config, ValidationResults results)
        {
            var missing = config.WorldSensors.Where(x => x.Key == null).ToArray();
            
            if (!missing.Any())
                return;
            
            results.AddError($"WorldSensors without Key: {string.Join(", ", missing.Select(x => x.Name))}");
        }
    }
}