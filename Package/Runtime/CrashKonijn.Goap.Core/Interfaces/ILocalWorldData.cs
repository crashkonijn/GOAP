namespace CrashKonijn.Goap.Core
{
    public interface ILocalWorldData : IWorldData
    {
        IGlobalWorldData GlobalData { get; }

        void SetParent(IGlobalWorldData globalData);
    }
}
