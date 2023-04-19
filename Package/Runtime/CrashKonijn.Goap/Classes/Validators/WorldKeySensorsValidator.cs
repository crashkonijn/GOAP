using System.Linq;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes.Validators
{
    public class WorldKeySensorsValidator : IValidator<IGoapSetConfig>
    {
        public void Validate(IGoapSetConfig config, ValidationResults results)
        {
            var required = config.GetWorldKeys();
            var provided = config.WorldSensors
                .Where(x => x.Key != null)
                .Select(x => x.Key)
                .ToArray();
            
            var missing = required.Except(provided).ToHashSet();

            if (!missing.Any())
                return;
            
            results.AddWarning($"WorldKeys without sensors: {string.Join(", ", missing.Select(x => x.Name))}");
        }
    }
}