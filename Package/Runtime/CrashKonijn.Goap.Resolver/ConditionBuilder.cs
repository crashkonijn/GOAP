using System.Collections.Generic;
using CrashKonijn.Goap.Core;

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
            var index = this.GetIndex(condition);

            if (index == -1)
                return this;

            this.conditionsMetList[index] = met;

            return this;
        }

        private int GetIndex(ICondition condition)
        {
            for (var i = 0; i < this.conditionIndexList.Count; i++)
            {
                if (this.conditionIndexList[i] == condition)
                    return i;
            }

            return -1;
        }

        public bool[] Build()
        {
            return this.conditionsMetList;
        }

        public void Clear()
        {
            for (var i = 0; i < this.conditionsMetList.Length; i++)
            {
                this.conditionsMetList[i] = false;
            }
        }
    }
}