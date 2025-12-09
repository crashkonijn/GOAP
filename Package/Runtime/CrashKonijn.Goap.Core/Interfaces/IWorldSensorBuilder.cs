using System;

namespace CrashKonijn.Goap.Core
{
    public interface IWorldSensorBuilder<T> where T : IWorldSensor
    {
        IWorldSensorBuilder<T> SetKey<TWorldKey>()
            where TWorldKey : IWorldKey;

        IWorldSensorBuilder<T> SetCallback(Action<T> callback);
        IWorldSensorConfig Build();
    }
}