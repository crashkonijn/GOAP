using System;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Demos.Complex.Actions;
using CrashKonijn.Goap.Demos.Complex.Behaviours;
using CrashKonijn.Goap.Demos.Complex.Classes.Items;
using CrashKonijn.Goap.Demos.Complex.Classes.Sources;
using CrashKonijn.Goap.Demos.Complex.Interfaces;
using CrashKonijn.Goap.Demos.Complex.Targets;
using CrashKonijn.Goap.Demos.Complex.WorldKeys;

namespace CrashKonijn.Goap.Demos.Complex.Factories.Extensions
{
    public static class ActionExtensions
    {
        public static void AddWanderAction(this AgentTypeBuilder builder)
        {
            builder.AddAction<WanderAction>()
                .SetTarget<WanderTarget>()
                .AddEffect<IsWandering>(EffectType.Increase)
                .SetProperties(new WanderAction.Props
                {
                    minTimer = 0.3f,
                    maxTimer = 1f
                });
        }
        
        public static void AddPickupItemAction<T>(this AgentTypeBuilder builder)
            where T : class, IHoldable
        {
            builder.AddAction<PickupItemAction<T>>()
                .SetTarget<ClosestTarget<T>>()
                .AddEffect<IsHolding<T>>(EffectType.Increase)
                .AddCondition<IsInWorld<T>>(Comparison.GreaterThanOrEqual, 1);
        }
        
        public static void AddGatherItemAction<TGatherable, TRequired>(this AgentTypeBuilder builder)
            where TGatherable : ItemBase, IGatherable
            where TRequired : IHoldable
        {
            builder.AddAction<GatherItemAction<TGatherable>>()
                .SetTarget<ClosestSourceTarget<TGatherable>>()
                .AddEffect<IsInWorld<TGatherable>>(EffectType.Increase)
                .AddCondition<IsHolding<TRequired>>(Comparison.GreaterThanOrEqual, 1);
        }
        
        public static void AddGatherItemSlowAction<TGatherable>(this AgentTypeBuilder builder)
            where TGatherable : ItemBase, IGatherable
        {
            builder.AddAction<GatherItemAction<TGatherable>>()
                .SetTarget<ClosestSourceTarget<TGatherable>>()
                .AddEffect<IsInWorld<TGatherable>>(EffectType.Increase)
                .SetBaseCost(3);
        }
        
        public static void AddCreateItemAction<T>(this AgentTypeBuilder builder)
            where T : ItemBase, ICreatable
        {
            var action = builder.AddAction<CreateItemAction<T>>()
                .SetTarget<ClosestTarget<AnvilSource>>()
                .AddEffect<CreatedItem<T>>(EffectType.Increase);
            
            if (typeof(T) == typeof(Axe))
            {
                action
                    .AddCondition<IsHolding<Iron>>(Comparison.GreaterThanOrEqual, 1)
                    .AddCondition<IsHolding<Wood>>(Comparison.GreaterThanOrEqual, 2);
                return;
            }
            
            if (typeof(T) == typeof(Pickaxe))
            {
                action
                    .AddCondition<IsHolding<Iron>>(Comparison.GreaterThanOrEqual, 2)
                    .AddCondition<IsHolding<Wood>>(Comparison.GreaterThanOrEqual, 1);
                return;
            }

            throw new Exception("No conditions set for this type of item!");
        }
        
        public static void AddHaulItemAction(this AgentTypeBuilder builder)
        {
            builder.AddAction<HaulItemAction>()
                .SetTarget<TransformTarget>()
                .AddEffect<ItemsOnFloor>(EffectType.Decrease)
                .AddCondition<ItemsOnFloor>(Comparison.GreaterThanOrEqual, 1);
        }
        
        public static void AddEatAction(this AgentTypeBuilder builder)
        {
            builder.AddAction<EatAction>()
                .SetTarget<TransformTarget>()
                .AddEffect<IsHungry>(EffectType.Decrease)
                .AddCondition<IsHolding<IEatable>>(Comparison.GreaterThanOrEqual, 1);
        }
    }
}