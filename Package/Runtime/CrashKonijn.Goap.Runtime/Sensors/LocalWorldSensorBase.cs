using System;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public abstract class LocalWorldSensorBase : ILocalWorldSensor
    {
        public IWorldKey Key => this.Config.Key;
        public virtual ISensorTimer Timer => SensorTimer.Always;
        public IWorldSensorConfig Config { get; private set; }
        public void SetConfig(IWorldSensorConfig config) => this.Config = config;

        public abstract void Created();

        public abstract void Update();
        public Type[] GetKeys() => new[] { this.Key.GetType() };

        public void Sense(IWorldData data, IActionReceiver agent, IComponentReference references)
        {
            var state = data.GetWorldState(this.Key.GetType());

            if (!this.Timer.ShouldSense(state?.Timer))
                return;

            data.SetState(this.Key, this.Sense(agent, references));
        }

        public virtual SenseValue Sense(IActionReceiver agent, IComponentReference references)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            return this.Sense(agent as IMonoAgent, references);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        [Obsolete("This should not be called anymore! Use 'Sense(IActionReceiver agent, IComponentReference references)' instead.")]
        public virtual SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            throw new GoapException("This should not be called anymore!");
        }
    }
}
