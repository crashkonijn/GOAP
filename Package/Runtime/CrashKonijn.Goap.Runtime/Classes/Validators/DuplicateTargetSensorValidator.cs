using System.Linq;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class DuplicateTargetSensorValidator : IValidator<IAgentTypeConfig>
    {
        public void Validate(IAgentTypeConfig config, IValidationResults results)
        {
            var provided = this.GetTargetSensorKeys(config).Concat(this.GetMultiSensorKeys(config));
            var duplicates = provided
                .GroupBy(x => x)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .ToArray();

            if (!duplicates.Any())
                return;

            results.AddWarning($"Duplicate target sensor keys: {string.Join(", ", duplicates)}");
        }

        private string[] GetTargetSensorKeys(IAgentTypeConfig agentTypeConfig)
        {
            return agentTypeConfig.TargetSensors
                .Where(x => x.Key != null)
                .Select(x => x.Key.Name)
                .ToArray();
        }

        private string[] GetMultiSensorKeys(IAgentTypeConfig agentTypeConfig)
        {
            var temp = new ClassResolver().Load<IMultiSensor, IMultiSensorConfig>(agentTypeConfig.MultiSensors);

            return temp
                .SelectMany(x => x.GetKeys())
                .Select(x => x.GetGenericTypeName())
                .ToArray();
        }
    }
}
