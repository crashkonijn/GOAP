namespace LamosInteractive.Goap.Interfaces
{
    public interface IActionKeyResolver
    {
        string GetKey(IAction action, ICondition condition);
        string GetKey(IAction action, IEffect effect);
    }
}