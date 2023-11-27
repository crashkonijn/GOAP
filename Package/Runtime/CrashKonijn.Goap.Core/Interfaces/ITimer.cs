namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface ITimer
    {
        void Touch();
        float GetElapsed();
        bool IsExpired(float time);
    }
}