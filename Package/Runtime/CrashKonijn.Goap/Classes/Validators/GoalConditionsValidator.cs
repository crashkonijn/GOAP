using System.Linq;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes.Validators
{
    public class GoalConditionsValidator : IValidator<IGoapSetConfig>
    {
        public void Validate(IGoapSetConfig config, ValidationResults results)
        {
            var missing = config.Goals.Where(x => !x.Conditions.Any()).ToArray();
            
            if (!missing.Any())
                return;
            
            results.AddWarning($"Goals without Conditions: {string.Join(", ", missing.Select(x => x.Name))}");
        }
    }
}