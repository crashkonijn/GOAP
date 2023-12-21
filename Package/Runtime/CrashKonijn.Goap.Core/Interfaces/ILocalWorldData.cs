namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface ILocalWorldData : IWorldData
    {
        IGlobalWorldData GlobalData { get; }
        
        void SetParent(IGlobalWorldData globalData);
    }
}