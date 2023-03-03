using System.Collections.Generic;
using System.Linq;
using LamosInteractive.Goap.Interfaces;

namespace CrashKonijn.Goap.Resolver
{
    public class ExecutableBuilder
    {
        private readonly List<IAction> actionIndexList;
        private bool[] executableList;

        public ExecutableBuilder(List<IAction> actionIndexList)
        {
            this.actionIndexList = actionIndexList;
            this.executableList = this.actionIndexList.Select(x => false).ToArray();
        }
        
        public ExecutableBuilder SetExecutable(IAction action, bool executable)
        {
            var index = this.actionIndexList.IndexOf(action);
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