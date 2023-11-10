using System;
using System.Collections.Generic;

namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface INode
    {
        Guid Guid { get; }
        IConnectable Action { get; set; }
        List<INodeEffect> Effects { get; set; }
        List<INodeCondition> Conditions { get; set; }
        bool IsRootNode { get; }
    }
}