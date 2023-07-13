using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.GOAP.Demos.TurnBased.Sensors.Target;
using CrashKonijn.Goap.Resolver;
using Demos.Complex.Targets;
using Demos.Complex.WorldKeys;
using Demos.Shared.Goals;
using Demos.TurnBased.Actions;

namespace CrashKonijn.GOAP.Demos.TurnBased.Factories
{
    public class TurnGoapSetFactory : GoapSetFactoryBase
    {
        public override IGoapSetConfig Create()
        {
            var builder = new GoapSetBuilder("TurnBased");
            
            builder
                .AddGoal<WanderGoal>()
                .AddCondition<IsWandering>(Comparison.GreaterThanOrEqual, 1);

            builder
                .AddAction<TurnWanderAction>()
                .SetTarget<WanderTarget>()
                .AddEffect<IsWandering>(true);

            builder
                .AddTargetSensor<TurnWanderTargetSensor>()
                .SetTarget<WanderTarget>();

            return builder.Build();
        }
    }
}