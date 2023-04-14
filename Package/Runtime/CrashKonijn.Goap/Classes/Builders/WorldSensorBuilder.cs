using System;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes.Builders
{
    public class WorldSensorBuilder
    {
        private readonly WorldKeyBuilder worldKeyBuilder;
        private readonly WorldSensorConfig config;

        public WorldSensorBuilder(Type type, WorldKeyBuilder worldKeyBuilder)
        {
            this.worldKeyBuilder = worldKeyBuilder;
            this.config = new WorldSensorConfig()
            {
                Name = type.Name,
                ClassType = type.AssemblyQualifiedName
            };
        }
        
        public WorldSensorBuilder SetKey(string key)
        {
            this.config.Key = this.worldKeyBuilder.GetKey(key);
            
            return this;
        }
        
        public WorldSensorBuilder SetKey<T1>(string key)
        {
            this.config.Key = this.worldKeyBuilder.GetKey<T1>(key);
            
            return this;
        }
        
        public WorldSensorBuilder SetKey<T1, T2>(string key)
        {
            this.config.Key = this.worldKeyBuilder.GetKey<T1, T2>(key);
            
            return this;
        }
        
        public IWorldSensorConfig Build()
        {
            return this.config;
        }

        public static WorldSensorBuilder Create<TWorldSensor>(WorldKeyBuilder worldKeyBuilder)
            where TWorldSensor : IWorldSensor
        {
            return new WorldSensorBuilder(typeof(TWorldSensor), worldKeyBuilder);
        }
    }
}