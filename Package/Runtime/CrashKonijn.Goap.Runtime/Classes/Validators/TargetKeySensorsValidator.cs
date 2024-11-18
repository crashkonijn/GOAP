using System.Linq;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class TargetKeySensorsValidator : IValidator<IAgentTypeConfig>
    {
        public void Validate(IAgentTypeConfig agentTypeConfig, IValidationResults results)
        {
            var required = agentTypeConfig.GetTargetKeys().Select(x => x.Name);
            var provided = this.GetTargetSensorKeys(agentTypeConfig).Concat(this.GetMultiSensorKeys(agentTypeConfig));

            var missing = required.Except(provided).ToHashSet();

            if (!missing.Any())
                return;

            results.AddWarning($"TargetKeys without sensors: {string.Join(", ", missing)}");
        }

        private string[] GetTargetSensorKeys(IAgentTypeConfig agentTypeConfig)
        {
            return agentTypeConfig.TargetSensors
                .Where(x => x.Key != null)
                .Select(x => x.Key.Name)
                .Distinct()
                .ToArray();
        }

        private string[] GetMultiSensorKeys(IAgentTypeConfig agentTypeConfig)
        {
            var temp = new ClassResolver().Load<IMultiSensor, IMultiSensorConfig>(agentTypeConfig.MultiSensors);

            return temp
                .SelectMany(x => x.GetKeys())
                .Select(x => x.GetGenericTypeName())
                .Distinct()
                .ToArray();
        }
    }
}
