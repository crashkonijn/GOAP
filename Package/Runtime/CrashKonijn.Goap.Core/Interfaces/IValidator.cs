namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IValidator<T>
    {
        void Validate(T config, IValidationResults results);
    }
}