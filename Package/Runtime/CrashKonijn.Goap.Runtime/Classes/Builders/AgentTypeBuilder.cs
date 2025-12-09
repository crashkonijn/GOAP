using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class AgentTypeBuilder
    {
        private readonly AgentTypeConfig agentTypeConfig;

        private readonly List<CapabilityBuilder> capabilityBuilders = new();
        private readonly List<ICapabilityConfig> capabilityConfigs = new();
        private readonly IGoapInjector injector;

        public AgentTypeBuilder(IGoapInjector injector, string name)
        {
            this.injector = injector;
            this.agentTypeConfig = new AgentTypeConfig(name);
        }

        [Obsolete("use AgentTypeFactoryBase.CreateBuilder(string name) instead")]
        public AgentTypeBuilder(string name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        ///     Creates a new capability with the specified name.
        /// </summary>
        /// <param name="name">The name of the capability.</param>
        /// <returns>A new instance of <see cref="CapabilityBuilder" />.</returns>
        public CapabilityBuilder CreateCapability(string name)
        {
            var capabilityBuilder = new CapabilityBuilder(name);

            this.capabilityBuilders.Add(capabilityBuilder);

            return capabilityBuilder;
        }

        /// <summary>
        ///     Creates a new capability with the specified name and applies the given callback.
        /// </summary>
        /// <param name="name">The name of the capability.</param>
        /// <param name="callback">The callback to apply to the capability builder.</param>
        public void CreateCapability(string name, Action<CapabilityBuilder> callback)
        {
            var capabilityBuilder = new CapabilityBuilder(name);

            callback(capabilityBuilder);

            this.capabilityBuilders.Add(capabilityBuilder);
        }

        /// <summary>
        ///     Adds a capability of the specified type.
        /// </summary>
        /// <typeparam name="TCapability">The type of the capability.</typeparam>
        public void AddCapability<TCapability>()
            where TCapability : CapabilityFactoryBase, new()
        {
            var capability = new TCapability();

            this.injector?.Inject(capability);

            this.capabilityConfigs.Add(capability.Create());
        }

        /// <summary>
        ///     Adds a capability using the specified capability factory.
        /// </summary>
        /// <param name="capabilityFactory">The capability factory.</param>
        public void AddCapability(CapabilityFactoryBase capabilityFactory)
        {
            this.injector?.Inject(capabilityFactory);

            this.capabilityConfigs.Add(capabilityFactory.Create());
        }

        /// <summary>
        ///     Adds a capability using the specified mono capability factory.
        /// </summary>
        /// <param name="capabilityFactory">The mono capability factory.</param>
        public void AddCapability(MonoCapabilityFactoryBase capabilityFactory)
        {
            this.injector?.Inject(capabilityFactory);

            this.capabilityConfigs.Add(capabilityFactory.Create());
        }

        /// <summary>
        ///     Adds a capability using the specified scriptable capability factory.
        /// </summary>
        /// <param name="capabilityFactory">The scriptable capability factory.</param>
        public void AddCapability(ScriptableCapabilityFactoryBase capabilityFactory)
        {
            this.injector?.Inject(capabilityFactory);

            this.capabilityConfigs.Add(capabilityFactory.Create());
        }

        /// <summary>
        ///     Adds a capability using the specified capability builder.
        /// </summary>
        /// <param name="capabilityBuilder">The capability builder.</param>
        public void AddCapability(CapabilityBuilder capabilityBuilder)
        {
            this.capabilityConfigs.Add(capabilityBuilder.Build());
        }

        /// <summary>
        ///     Adds a capability using the specified capability config.
        /// </summary>
        /// <param name="capabilityConfig">The capability config.</param>
        public void AddCapability(ICapabilityConfig capabilityConfig)
        {
            this.capabilityConfigs.Add(capabilityConfig);
        }

        /// <summary>
        ///     Builds the agent type configuration.
        /// </summary>
        /// <returns>The built <see cref="AgentTypeConfig" />.</returns>
        public AgentTypeConfig Build()
        {
            this.capabilityConfigs.AddRange(this.capabilityBuilders.Select(x => x.Build()));

            this.agentTypeConfig.Actions = this.capabilityConfigs.SelectMany(x => x.Actions).ToList();
            this.agentTypeConfig.Goals = this.capabilityConfigs.SelectMany(x => x.Goals).ToList();
            this.agentTypeConfig.TargetSensors = this.capabilityConfigs.SelectMany(x => x.TargetSensors).ToList();
            this.agentTypeConfig.WorldSensors = this.capabilityConfigs.SelectMany(x => x.WorldSensors).ToList();
            this.agentTypeConfig.MultiSensors = this.capabilityConfigs.SelectMany(x => x.MultiSensors).ToList();

            return this.agentTypeConfig;
        }
    }
}