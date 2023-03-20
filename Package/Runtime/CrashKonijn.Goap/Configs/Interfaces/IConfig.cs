namespace CrashKonijn.Goap.Configs.Interfaces
{
    public interface IConfig
    {
        public string Name { get; }
    }

    public interface IClassConfig : IConfig
    {
        public string ClassType { get; set; }
    }
}