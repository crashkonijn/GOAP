using CrashKonijn.Goap.Classes.Validators;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IValidator<T>
    {
        void Validate(T config, ValidationResults results);
    }
}