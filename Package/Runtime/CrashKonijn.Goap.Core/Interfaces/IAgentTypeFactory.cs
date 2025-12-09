namespace CrashKonijn.Goap.Core
{
    public interface IAgentTypeFactory
    {
        void Construct(IGoapConfig config);
        IAgentTypeConfig Create();
    }
}