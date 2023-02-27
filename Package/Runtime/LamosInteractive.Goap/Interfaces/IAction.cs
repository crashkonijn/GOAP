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
        List<IEffect> Effects { get; }
        List<ICondition> Conditions { get; }
    }
}
