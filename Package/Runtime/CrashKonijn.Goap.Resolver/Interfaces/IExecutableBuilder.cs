namespace CrashKonijn.Goap.Resolver.Interfaces
{
    public interface IExecutableBuilder
    {
        IExecutableBuilder SetExecutable(IAction action, bool executable);
        void Clear();
        bool[] Build();
    }
}