using CrashKonijn.Goap.Classes.Validators;
using CrashKonijn.Goap.Configs.Interfaces;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IGoapSetConfigValidatorRunner
    {
        ValidationResults Validate(IGoapSetConfig goapSetConfig);
    }
}