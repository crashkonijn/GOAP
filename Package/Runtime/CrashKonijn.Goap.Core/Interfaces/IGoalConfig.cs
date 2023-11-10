using System.Collections.Generic;

namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IGoalConfig : IClassConfig
    {
        int BaseCost { get; set; }
        List<ICondition> Conditions { get; }
    }
}