using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Resolver.Interfaces;

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
            var index = this.actionIndexList.IndexOf(action);

            if (index == -1)
                return this;
            
            this.costList[index] = cost;

            return this;
        }
        
        public float[] Build()
        {
            return this.costList;
        }

        public void Clear()
        {
            this.costList = this.actionIndexList.Select(x => 1f).ToArray();
        }
    }
}