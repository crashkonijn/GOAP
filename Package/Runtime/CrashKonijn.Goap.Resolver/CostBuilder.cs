using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Resolver
{
    public class CostBuilder : ICostBuilder
    {
        private readonly List<IConnectable> actionIndexList;
        private float[] costList;

        public CostBuilder(List<IConnectable> actionIndexList)
        {
            this.actionIndexList = actionIndexList;
            this.costList = this.actionIndexList.Select(x => 1f).ToArray();
        }

        public ICostBuilder SetCost(IConnectable action, float cost)
        {
            var index = this.GetIndex(action);

            if (index == -1)
                return this;

            this.costList[index] = cost;

            return this;
        }

        private int GetIndex(IConnectable condition)
        {
            for (var i = 0; i < this.actionIndexList.Count; i++)
            {
                if (this.actionIndexList[i] == condition)
                    return i;
            }

            return -1;
        }

        public float[] Build()
        {
            return this.costList;
        }

        public void Clear()
        {
            for (var i = 0; i < this.costList.Length; i++)
            {
                this.costList[i] = 1f;
            }
        }
    }
}