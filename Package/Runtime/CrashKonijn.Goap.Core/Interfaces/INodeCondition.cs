namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface INodeCondition
    {
        ICondition Condition { get; set; }
        INode[] Connections { get; set; }
    }
}