using System;
using System.Linq;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Resolvers;

namespace CrashKonijn.Goap.Classes.Validators
{
    public class WorldKeySensorsValidator : IValidator<IAgentTypeConfig>
    {
        public void Validate(IAgentTypeConfig agentTypeConfig, IValidationResults results)
        {
            var required = agentTypeConfig.GetWorldKeys().Select(x => x.Name).Distinct();
            var provided = this.GetWorldSensorKeys(agentTypeConfig).Concat(this.GetMultiSensorKeys(agentTypeConfig));
            
            var missing = required.Except(provided).ToHashSet();

            if (!missing.Any())
                return;
            
            results.AddWarning($"WorldKeys without sensors: {string.Join(", ", missing)}");
        }

        private string[] GetWorldSensorKeys(IAgentTypeConfig agentTypeConfig)
        {
            return agentTypeConfig.WorldSensors
                .Where(x => x.Key != null)
                .Select(x => x.Key.Name)
                .Distinct()
                .ToArray();
        }
        
        private string [] GetMultiSensorKeys(IAgentTypeConfig agentTypeConfig)
        {
            var temp = new ClassResolver().Load<IMultiSensor, IMultiSensorConfig>(agentTypeConfig.MultiSensors);
            
            return temp
                .SelectMany(x => x.GetKeys())
                .Select(x => x.Name)
                .Distinct()
                .ToArray();
        }
    }
}