namespace CrashKonijn.Goap.Interfaces
{
    public interface IDataReferenceInjector : IComponentReference
    {
        void Inject(IActionData data);
    }
}