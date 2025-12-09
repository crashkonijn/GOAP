using System;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public interface ITargetSensorBuilder<T> where T : ITargetSensor
    {
        ITargetSensorBuilder<T> SetTarget<TTarget>()
            where TTarget : ITargetKey;

        ITargetSensorBuilder<T> SetCallback(Action<T> callback);
        ITargetSensorConfig Build();
    }
}