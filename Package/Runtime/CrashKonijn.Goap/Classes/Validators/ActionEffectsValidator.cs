using System.Linq;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes.Validators
{
    public class ActionEffectsValidator : IValidator<IGoapSetConfig>
    {
        public void Validate(IGoapSetConfig goapSetConfig, ValidationResults results)
        {
            var missing = goapSetConfig.Actions.Where(x => !x.Effects.Any()).ToArray();
            
            if (!missing.Any())
                return;
            
            results.AddWarning($"Actions without Effects: {string.Join(", ", missing.Select(x => x.Name))}");
        }
    }
}