using CrashKonijn.Goap.Configs.Interfaces;

namespace CrashKonijn.Goap.Classes.Validators
{
    public interface IGoapSetConfigValidatorRunner
    {
        ValidationResults Validate(IGoapSetConfig config);
    }
}