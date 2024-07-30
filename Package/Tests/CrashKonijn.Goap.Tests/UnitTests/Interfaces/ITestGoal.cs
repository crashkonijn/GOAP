using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.UnitTests.Interfaces
{
    public interface ITestGoal : IGoal
    {
        public string Name { get; set; }
    }
}