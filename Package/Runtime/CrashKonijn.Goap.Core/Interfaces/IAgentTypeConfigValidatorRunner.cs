namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IAgentTypeConfigValidatorRunner
    {
        IValidationResults Validate(IAgentTypeConfig agentTypeConfig);
    }
}