using System;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class WorldSensorBuilder<T> : WorldSensorBuilder
        where T : IWorldSensor
    {
        public WorldSensorBuilder(WorldKeyBuilder worldKeyBuilder) : base(typeof(T), worldKeyBuilder) { }
        
        public WorldSensorBuilder<T> SetKey<TWorldKey>()
            where TWorldKey : IWorldKey
        {
            this.config.Key = this.worldKeyBuilder.GetKey<TWorldKey>();

            return this;
        }
        
        public WorldSensorBuilder<T> SetCallback(Action<T> callback)
        {
            this.config.Callback = (obj) => callback((T) obj);
            return this;
        }
    }
    
    public class WorldSensorBuilder
    {
        protected readonly WorldKeyBuilder worldKeyBuilder;
        protected readonly WorldSensorConfig config;

        public WorldSensorBuilder(Type type, WorldKeyBuilder worldKeyBuilder)
        {
            this.worldKeyBuilder = worldKeyBuilder;
            this.config = new WorldSensorConfig()
            {
                Name = type.Name,
                ClassType = type.AssemblyQualifiedName,
            };
        }

        public IWorldSensorConfig Build()
        {
            return this.config;
        }

        public static WorldSensorBuilder<TWorldSensor> Create<TWorldSensor>(WorldKeyBuilder worldKeyBuilder)
            where TWorldSensor : IWorldSensor
        {
            return new WorldSensorBuilder<TWorldSensor>(worldKeyBuilder);
        }
    }
}
