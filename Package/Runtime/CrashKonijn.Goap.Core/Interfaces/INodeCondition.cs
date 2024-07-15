namespace CrashKonijn.Goap.Core
{
    public interface INodeCondition
    {
        ICondition Condition { get; set; }
        INode[] Connections { get; set; }
    }
}