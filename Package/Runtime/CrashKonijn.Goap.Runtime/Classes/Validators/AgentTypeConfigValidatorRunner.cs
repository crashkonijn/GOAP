using System.Collections.Generic;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class AgentTypeConfigValidatorRunner : IAgentTypeConfigValidatorRunner
    {
        private readonly List<IValidator<IAgentTypeConfig>> validators = new()
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
            new TargetSensorKeyValidator(),
            new DuplicateTargetSensorValidator(),
            new DuplicateWorldSensorValidator(),
        };

        public IValidationResults Validate(IAgentTypeConfig agentTypeConfig)
        {
            var results = new ValidationResults(agentTypeConfig.Name);

            foreach (var validator in this.validators)
            {
                validator.Validate(agentTypeConfig, results);
            }

            return results;
        }
    }
}
