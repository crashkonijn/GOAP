namespace CrashKonijn.Agent.Core
{
    public interface ITimer
    {
        void Touch();
        float GetElapsed();
        bool IsRunningFor(float time);
    }
}