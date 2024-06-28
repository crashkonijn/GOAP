using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Resolver
{
    public class ExecutableBuilder : IExecutableBuilder
    {
        private readonly List<IConnectable> actionIndexList;
        private bool[] executableList;

        public ExecutableBuilder(List<IConnectable> actionIndexList)
        {
            this.actionIndexList = actionIndexList;
            this.executableList = this.actionIndexList.Select(x => false).ToArray();
        }

        public IExecutableBuilder SetExecutable(IConnectable action, bool executable)
        {
            var index = this.GetIndex(action);

            if (index == -1)
                return this;

            this.executableList[index] = executable;

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
            for (var i = 0; i < this.executableList.Length; i++)
            {
                this.executableList[i] = false;
            }
        }

        public bool[] Build()
        {
            return this.executableList;
        }
    }
}