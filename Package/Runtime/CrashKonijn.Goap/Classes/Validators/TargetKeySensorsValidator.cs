using System.Linq;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes.Validators
{
    public class TargetKeySensorsValidator : IValidator<IGoapSetConfig>
    {
        public void Validate(IGoapSetConfig config, ValidationResults results)
        {
            var required = config.GetTargetKeys();
            var provided = config.TargetSensors
                .Where(x => x.Key != null)
                .Select(x => x.Key)
                .ToArray();
            
            var missing = required.Except(provided).ToHashSet();
            
            if (!missing.Any())
                return;
            
            results.AddWarning($"TargetKeys without sensors: {string.Join(", ", missing.Select(x => x.Name))}");
        }
    }
}