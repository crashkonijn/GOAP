namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IGoapController
    {
        void Initialize(IGoap goap);
        void OnUpdate();
        void OnLateUpdate();
    }
}