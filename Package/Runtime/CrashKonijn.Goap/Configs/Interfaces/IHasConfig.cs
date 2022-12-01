namespace CrashKonijn.Goap.Configs.Interfaces
{
    public interface IHasConfig<TConfig>
        where TConfig : IConfig
    {
        public TConfig Config { get; }
        public void SetConfig(TConfig config);
    }
}