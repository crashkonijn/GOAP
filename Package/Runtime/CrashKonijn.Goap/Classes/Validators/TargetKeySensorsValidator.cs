using System.Linq;
using CrashKonijn.Goap.Core.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Classes.Validators
{
    public class TargetKeySensorsValidator : IValidator<IAgentTypeConfig>
    {
        public void Validate(IAgentTypeConfig agentTypeConfig, IValidationResults results)
        {
            var required = agentTypeConfig.GetTargetKeys().Select(x => x.Name);
            var provided = agentTypeConfig.TargetSensors
                .Where(x => x.Key != null)
                .Select(x => x.Key.Name)
                .ToArray();
            
            var missing = required.Except(provided).ToHashSet();
            
            if (!missing.Any())
                return;
            
            results.AddWarning($"TargetKeys without sensors: {string.Join(", ", missing)}");
        }
    }
}