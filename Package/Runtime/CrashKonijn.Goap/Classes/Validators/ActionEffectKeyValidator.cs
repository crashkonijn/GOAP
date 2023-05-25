using System.Linq;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes.Validators
{
    public class ActionEffectKeyValidator : IValidator<IGoapSetConfig>
    {
        public void Validate(IGoapSetConfig goapSetConfig, ValidationResults results)
        {
            foreach (var configAction in goapSetConfig.Actions)
            {
                var missing = configAction.Effects.Where(x => x.WorldKey == null).ToArray();
                
                if (!missing.Any())
                    continue;
                
                results.AddError($"Action {configAction.Name} has effects without WorldKey");
            }
        }
    }
}