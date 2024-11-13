using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Demos.TurnBased;
using CrashKonijn.GOAP.Demos.TurnBased.Sensors.Target;
using CrashKonijn.Goap.Runtime;
using TurnBased.Actions;

namespace CrashKonijn.GOAP.Demos.TurnBased.Factories
{
    public class TurnGoapSetFactory : AgentTypeFactoryBase
    {
        public override IAgentTypeConfig Create()
        {
            var builder = new AgentTypeBuilder("TurnBased");

            builder.CreateCapability("turn-based-capability", (capability) =>
            {
                capability
                    .AddGoal<WanderGoal>()
                    .AddCondition<IsWandering>(Comparison.GreaterThanOrEqual, 1);

                capability
                    .AddAction<TurnWanderAction>()
                    .SetTarget<WanderTarget>()
                    .AddEffect<IsWandering>(EffectType.Increase);

                capability
                    .AddTargetSensor<TurnWanderTargetSensor>()
                    .SetTarget<WanderTarget>();
            });

            return builder.Build();
        }
    }
}
