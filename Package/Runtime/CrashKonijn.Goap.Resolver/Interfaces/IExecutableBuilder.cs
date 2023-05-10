namespace CrashKonijn.Goap.Resolver.Interfaces
{
    public interface IExecutableBuilder
    {
        ExecutableBuilder SetExecutable(IAction action, bool executable);
        void Clear();
        bool[] Build();
    }
}