using System;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Demos.Complex.Actions;
using CrashKonijn.Goap.Demos.Complex.Behaviours;
using CrashKonijn.Goap.Demos.Complex.Classes.Items;
using CrashKonijn.Goap.Demos.Complex.Classes.Sources;
using CrashKonijn.Goap.Demos.Complex.Interfaces;
using CrashKonijn.Goap.Demos.Complex.Targets;
using CrashKonijn.Goap.Demos.Complex.WorldKeys;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.Demos.Complex.Factories.Extensions
{
    public static class ActionExtensions
    {
        public static void AddPickupItemAction<T>(this CapabilityBuilder builder)
            where T : class, IHoldable
        {
            builder.AddAction<PickupItemAction<T>>()
                .SetTarget<ClosestTarget<T>>()
                .AddEffect<IsHolding<T>>(EffectType.Increase)
                .AddCondition<IsInWorld<T>>(Comparison.GreaterThanOrEqual, 1);
        }
        
        public static void AddGatherItemAction<TGatherable, TRequired>(this CapabilityBuilder builder)
            where TGatherable : ItemBase, IGatherable
            where TRequired : IHoldable
        {
            builder.AddAction<GatherItemAction<TGatherable>>()
                .SetTarget<ClosestSourceTarget<TGatherable>>()
                .AddEffect<IsInWorld<TGatherable>>(EffectType.Increase)
                .AddCondition<IsHolding<TRequired>>(Comparison.GreaterThanOrEqual, 1)
                .SetProperties(new GatherItemAction<TGatherable>.Props
                {
                    pickupItem = false,
                    timer = 3
                });
        }
        
        public static void AddGatherItemSlowAction<TGatherable>(this CapabilityBuilder builder)
            where TGatherable : ItemBase, IGatherable
        {
            builder.AddAction<GatherItemAction<TGatherable>>()
                .SetTarget<ClosestSourceTarget<TGatherable>>()
                .AddEffect<IsInWorld<TGatherable>>(EffectType.Increase)
                .SetBaseCost(5)
                .SetProperties(new GatherItemAction<TGatherable>.Props
                {
                    pickupItem = false,
                    timer = 5
                });
        }
        
        public static void AddCreateItemAction<T>(this CapabilityBuilder builder, int requiredIron, int requiredWood)
            where T : ItemBase, ICreatable
        {
            var action = builder.AddAction<CreateItemAction<T>>()
                .SetTarget<ClosestTarget<AnvilSource>>()
                .AddEffect<CreatedItem<T>>(EffectType.Increase)
                .SetProperties(new CreateItemAction<T>.Props
                {
                    requiredIron = requiredIron,
                    requiredWood = requiredWood
                });
            
            if (requiredIron > 0)
                action.AddCondition<IsHolding<Iron>>(Comparison.GreaterThanOrEqual, requiredIron);
            
            if (requiredWood > 0)
                action.AddCondition<IsHolding<Wood>>(Comparison.GreaterThanOrEqual, requiredWood);
        }
    }
}