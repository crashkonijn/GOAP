using System;
using System.Collections.Generic;

namespace LamosInteractive.Goap.Interfaces
{
    /*
     * This interface is used to identify an action. 
     */
    public interface IAction
    {
        Guid Guid { get; }
        HashSet<IEffect> Effects { get; }
        HashSet<ICondition> Conditions { get; }
    }
}
