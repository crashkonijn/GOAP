namespace CrashKonijn.Goap.Core
{
    public interface ISensorRunner
    {
        void Update();
        void Update(IGoapAction action);
        void SenseGlobal();
        void SenseGlobal(IGoapAction action);
        void SenseLocal(IMonoGoapActionProvider actionProvider);
        void SenseLocal(IMonoGoapActionProvider actionProvider, IGoapAction action);
        void SenseLocal(IMonoGoapActionProvider actionProvider, IGoal goal);
        void SenseLocal(IMonoGoapActionProvider actionProvider, IGoalRequest goalRequest);
        void InitializeGraph(IGraph graph);
    }
}