using CrashKonijn.Agent.Core;

namespace CrashKonijn.Goap.Core
{
    public interface IGoapAction : IAction, IConnectable, IHasConfig<IActionConfig>
    {
        
    }
}