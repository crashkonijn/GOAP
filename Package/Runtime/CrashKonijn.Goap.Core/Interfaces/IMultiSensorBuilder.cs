using System;

namespace CrashKonijn.Goap.Core
{
    public interface IMultiSensorBuilder<T> where T : IMultiSensor
    {
        IMultiSensorBuilder<T> SetCallback(Action<T> callback);
        IMultiSensorConfig Build();
    }
}