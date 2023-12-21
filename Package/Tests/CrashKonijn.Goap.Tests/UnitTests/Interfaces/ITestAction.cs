using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.UnitTests.Interfaces
{
    public interface ITestAction : IAction
    {
        public string Name { get; set; }
    }
}