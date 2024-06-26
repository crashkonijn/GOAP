using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.UnitTests.Interfaces
{
    public interface ITestAction : IGoapAction
    {
        public string Name { get; set; }
    }
}