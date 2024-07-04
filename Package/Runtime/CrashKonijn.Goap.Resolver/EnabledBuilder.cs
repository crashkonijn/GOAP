using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Resolver
{
    public class EnabledBuilder : IEnabledBuilder
    {
        private readonly List<IConnectable> actionIndexList;
        private bool[] enabledList;

        public EnabledBuilder(List<IConnectable> actionIndexList)
        {
            this.actionIndexList = actionIndexList;
            this.enabledList = this.actionIndexList.Select(x => true).ToArray();
        }

        public IEnabledBuilder SetEnabled(IConnectable action, bool executable)
        {
            var index = this.GetIndex(action);

            if (index == -1)
                return this;

            this.enabledList[index] = executable;

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

        public void Clear()
        {
            for (var i = 0; i < this.enabledList.Length; i++)
            {
                this.enabledList[i] = true;
            }
        }

        public bool[] Build()
        {
            return this.enabledList;
        }
    }
}