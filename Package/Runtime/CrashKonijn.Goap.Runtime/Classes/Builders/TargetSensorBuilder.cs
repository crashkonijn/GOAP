using System;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class TargetSensorBuilder<T> : TargetSensorBuilder
        where T : ITargetSensor
    {
        public TargetSensorBuilder(TargetKeyBuilder targetKeyBuilder) : base(typeof(T), targetKeyBuilder) { }
        
        public TargetSensorBuilder<T> SetTarget<TTarget>()
            where TTarget : ITargetKey
        {
            this.config.Key = this.targetKeyBuilder.GetKey<TTarget>();

            return this;
        }
        
        public TargetSensorBuilder<T> SetCallback(Action<T> callback)
        {
            this.config.Callback = (obj) => callback((T) obj);
            return this;
        }
    }
    
    public class TargetSensorBuilder
    {
        protected readonly TargetKeyBuilder targetKeyBuilder;
        protected readonly TargetSensorConfig config;

        public TargetSensorBuilder(Type type, TargetKeyBuilder targetKeyBuilder)
        {
            this.targetKeyBuilder = targetKeyBuilder;
            this.config = new TargetSensorConfig()
            {
                Name = type.Name,
                ClassType = type.AssemblyQualifiedName,
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
