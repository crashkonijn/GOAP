using System.Linq;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes.Validators
{
    public class WorldSensorKeyValidator : IValidator<IGoapSetConfig>
    {
        public void Validate(IGoapSetConfig goapSetConfig, ValidationResults results)
        {
            var missing = goapSetConfig.WorldSensors.Where(x => x.Key == null).ToArray();
            
            if (!missing.Any())
                return;
            
            results.AddError($"WorldSensors without Key: {string.Join(", ", missing.Select(x => x.Name))}");
        }
    }
}