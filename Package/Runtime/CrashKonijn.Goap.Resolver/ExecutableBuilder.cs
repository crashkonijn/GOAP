using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Resolver.Interfaces;

namespace CrashKonijn.Goap.Resolver
{
    public class ExecutableBuilder : IExecutableBuilder
    {
        private readonly List<IAction> actionIndexList;
        private bool[] executableList;

        public ExecutableBuilder(List<IAction> actionIndexList)
        {
            this.actionIndexList = actionIndexList;
            this.executableList = this.actionIndexList.Select(x => false).ToArray();
        }
        
        public IExecutableBuilder SetExecutable(IAction action, bool executable)
        {
            var index = this.actionIndexList.IndexOf(action);

            if (index == -1)
                return this;
            
            this.executableList[index] = executable;

            return this;
        }

        public void Clear()
        {
            this.executableList = this.actionIndexList.Select(x => false).ToArray();
        }
        
        public bool[] Build()
        {
            return this.executableList;
        }
    }
}