namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface INodeEffect
    {
        IEffect Effect { get; set; }
        INode[] Connections { get; set; }
    }
}