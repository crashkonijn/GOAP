using System;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class MultiSensorBuilder
    {
        private readonly WorldKeyBuilder worldKeyBuilder;
        private readonly MultiSensorConfig config;

        public MultiSensorBuilder(Type type, WorldKeyBuilder worldKeyBuilder)
        {
            this.worldKeyBuilder = worldKeyBuilder;
            this.config = new MultiSensorConfig()
            {
                Name = type.Name,
                ClassType = type.AssemblyQualifiedName
            };
        }

        public IMultiSensorConfig Build()
        {
            return this.config;
        }

        public static MultiSensorBuilder Create<TMultiSensor>(WorldKeyBuilder worldKeyBuilder)
            where TMultiSensor : IMultiSensor
        {
            return new MultiSensorBuilder(typeof(TMultiSensor), worldKeyBuilder);
        }
    }
}