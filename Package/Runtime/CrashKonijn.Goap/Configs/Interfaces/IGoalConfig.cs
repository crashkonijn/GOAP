using System.Collections.Generic;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Configs.Interfaces
{
    public interface IGoalConfig : IClassConfig
    {
        int BaseCost { get; set; }
        List<ICondition> Conditions { get; }
    }
}