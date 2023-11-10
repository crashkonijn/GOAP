using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.UnitTests.Interfaces
{
    public interface ITestGoal : IGoal
    {
        public string Name { get; set; }
    }
}