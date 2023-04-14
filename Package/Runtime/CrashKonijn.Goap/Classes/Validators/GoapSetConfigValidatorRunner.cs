using System.Collections.Generic;
using CrashKonijn.Goap.Configs.Interfaces;

namespace CrashKonijn.Goap.Classes.Validators
{
    public class GoapSetConfigValidatorRunner : IGoapSetConfigValidatorRunner
    {
        private readonly List<IValidator<IGoapSetConfig>> validators = new ()
        {
            new WorldKeySensorsValidator(),
            new TargetKeySensorsValidator(),
            new ActionClassTypeValidator(),
            new GoalClassTypeValidator(),
            new TargetSensorClassTypeValidator(),
            new WorldSensorClassTypeValidator(),
            new ActionEffectsValidator(),
            new GoalConditionsValidator(),
            new ActionTargetValidator(),
            new ActionEffectKeyValidator(),
            new ActionConditionKeyValidator(),
            new GoalConditionsValidator(),
            new GoalConditionKeyValidator()
        };
        
        public ValidationResults Validate(IGoapSetConfig config)
        {
            var results = new ValidationResults();
            
            foreach (var validator in this.validators)
            {
                validator.Validate(config, results);
            }

            return results;
        }
    }
}