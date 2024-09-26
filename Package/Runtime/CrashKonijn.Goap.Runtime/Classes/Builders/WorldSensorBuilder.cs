using System;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
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
                ClassType = type.AssemblyQualifiedName,
            };
        }

        public WorldSensorBuilder SetKey<TWorldKey>()
            where TWorldKey : IWorldKey
        {
            this.config.Key = this.worldKeyBuilder.GetKey<TWorldKey>();

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
