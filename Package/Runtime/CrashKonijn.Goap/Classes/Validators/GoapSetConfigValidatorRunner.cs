using System.Collections.Generic;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

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
            new GoalConditionKeyValidator(),
            new WorldSensorKeyValidator(),
            new TargetSensorKeyValidator()
        };
        
        public ValidationResults Validate(IGoapSetConfig goapSetConfig)
        {
            var results = new ValidationResults(goapSetConfig.Name);
            
            foreach (var validator in this.validators)
            {
                validator.Validate(goapSetConfig, results);
            }

            return results;
        }
    }
}