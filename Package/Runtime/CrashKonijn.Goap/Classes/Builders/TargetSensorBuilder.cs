using System;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes.Builders
{
    public class TargetSensorBuilder
    {
        private readonly TargetKeyBuilder targetKeyBuilder;
        private readonly TargetSensorConfig config;

        public TargetSensorBuilder(Type type, TargetKeyBuilder targetKeyBuilder)
        {
            this.targetKeyBuilder = targetKeyBuilder;
            this.config = new TargetSensorConfig()
            {
                Name = type.Name,
                ClassType = type.AssemblyQualifiedName
            };
        }
        
        public TargetSensorBuilder SetTarget(string key)
        {
            this.config.Key = this.targetKeyBuilder.GetKey(key);
            
            return this;
        }
        
        public TargetSensorBuilder SetTarget<T1>(string key)
        {
            this.config.Key = this.targetKeyBuilder.GetKey<T1>(key);
            
            return this;
        }
        
        public TargetSensorBuilder SetTarget<T1, T2>(string key)
        {
            this.config.Key = this.targetKeyBuilder.GetKey<T1, T2>(key);
            
            return this;
        }
        
        public ITargetSensorConfig Build()
        {
            return this.config;
        }

        public static TargetSensorBuilder Create<TTargetSensor>(TargetKeyBuilder targetKeyBuilder) where TTargetSensor : ITargetSensor
        {
            return new TargetSensorBuilder(typeof(TTargetSensor), targetKeyBuilder);
        }
    }
}