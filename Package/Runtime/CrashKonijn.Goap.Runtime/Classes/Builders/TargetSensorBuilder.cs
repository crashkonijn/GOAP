using System;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class TargetSensorBuilder<T> : TargetSensorBuilder, ITargetSensorBuilder<T> where T : ITargetSensor
    {
        public TargetSensorBuilder(TargetKeyBuilder targetKeyBuilder) : base(typeof(T), targetKeyBuilder)
        {
        }

        public ITargetSensorBuilder<T> SetTarget<TTarget>()
            where TTarget : ITargetKey
        {
            this.config.Key = this.targetKeyBuilder.GetKey<TTarget>();

            return this;
        }

        public ITargetSensorBuilder<T> SetCallback(Action<T> callback)
        {
            this.config.Callback = obj => callback((T)obj);
            return this;
        }
    }

    public class TargetSensorBuilder
    {
        protected readonly TargetSensorConfig config;
        protected readonly TargetKeyBuilder targetKeyBuilder;

        public TargetSensorBuilder(Type type, TargetKeyBuilder targetKeyBuilder)
        {
            this.targetKeyBuilder = targetKeyBuilder;
            this.config = new TargetSensorConfig
            {
                Name = type.Name,
                ClassType = type.AssemblyQualifiedName
            };
        }

        public ITargetSensorConfig Build()
        {
            return this.config;
        }

        public static TargetSensorBuilder<TTargetSensor> Create<TTargetSensor>(TargetKeyBuilder targetKeyBuilder) where TTargetSensor : ITargetSensor
        {
            return new TargetSensorBuilder<TTargetSensor>(targetKeyBuilder);
        }
    }
}