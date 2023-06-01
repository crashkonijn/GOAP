using System.Collections.Generic;
using CrashKonijn.Goap.Resolver.Interfaces;

namespace CrashKonijn.Goap.Resolver
{
    public class ConditionBuilder : IConditionBuilder
    {
        private readonly List<ICondition> conditionIndexList;
        private bool[] conditionsMetList;
        
        public ConditionBuilder(List<ICondition> conditionIndexList)
        {
            this.conditionIndexList = conditionIndexList;
            this.conditionsMetList = new bool[this.conditionIndexList.Count];
        }
        
        public IConditionBuilder SetConditionMet(ICondition condition, bool met)
        {
            var index = this.conditionIndexList.FindIndex(x => x == condition);

            if (index == -1)
                return this;
            
            this.conditionsMetList[index] = met;

            return this;
        }
        
        public bool[] Build()
        {
            return this.conditionsMetList;
        }
        
        public void Clear()
        {
            this.conditionsMetList = new bool[this.conditionIndexList.Count];
        }
    }
}