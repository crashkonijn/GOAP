namespace CrashKonijn.Goap.Classes.Validators
{
    public interface IValidator<T>
    {
        void Validate(T config, ValidationResults results);
    }
}