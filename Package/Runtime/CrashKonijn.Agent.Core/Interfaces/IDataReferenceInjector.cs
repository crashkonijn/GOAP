namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IDataReferenceInjector : IComponentReference
    {
        void Inject(IActionData data);
    }
}