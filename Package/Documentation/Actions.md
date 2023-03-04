﻿# Actions

An action is a single step an agent can take to reach a goal. An action has requirements and effects based on which actions are chained together.

An action consists of 3 parts:
- Config
- Action class
- Action data

## Action config
A config needs to be created. This config consists of a couple settings needed to run the action and connect it in the GOAP graph.

### Conditions
Conditions is a list of world states that need to be true in order to perform this action. Each condition references a world key, and whether the value should be true or false.

### Effects
Effects is a list of world states that will be true (or false) after performing this action. Each effect references a world key, and whether the resulting value will be true or false.

### BaseCost
The base cost of this action. This is the cost of the action itself, without any additional costs added by the planner (distance for example).

### Target
Each action has a target position. An agent will first move towards this position before performing the action. A position is represented by a TargetKey. For example `ClosestApple` or `ClosestEnemy`.

### InRange
If the agent is not in range of the target position, it will move towards it. This value determines how close the agent needs to be to the target position before performing the action.

## Action data
The action data is used to store the state of the action for a single agent. This data is not persistent between agents or between multiple runs of the same action.

## Action class
An action always inherits from the `ActionBase<TData>` class. The generic type is the action data class. The action data class is used to store the state of the action. The action class itself should be stateless, since only one instance is used to perform the same action on multiple agents.

### ActionRunState
The `ActionRunState` is an enum that determines the state of the action. It can be one of the following values: `Continue` or `Stop`.

### Examples
{% code title="WanderAction.cs" lineNumbers="true" %}
```csharp
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace Demos.Actions
{
    [CreateAssetMenu(menuName = "Goap/Actions/WanderAction")]
    public class WanderAction : ActionBase<WanderAction.Data>
    {
        // This method is called before the action is performed. It is used to initialize the action data.
        public override Data CreateData()
        {
            return new Data();
        }

        // This method is called when the action is started. It is used to initialize the action.
        public override void OnStart(IMonoAgent agent, Data data)
        {
            
        }

        // This method is called every frame while the action is running. It is used to perform the action.
        public override ActionRunState Perform(IMonoAgent agent, Data data)
        {
            return ActionRunState.Stop;
        }

        // This method is called when the action is stopped. It is used to clean up the action.
        public override void OnEnd(IMonoAgent agent, Data data)
        {
        }

        // Action data class. It stores the state of the action for a single agent. This data is not persistent between agents or between multiple runs of the same action.
        public class Data : IActionData
        {
            // The target position of the action. This is set by the planner.
            public ITarget Target { get; set; }
        }
    }
}
```
{% endcode %}

{% code title="EatAppleAction.cs" lineNumbers="true" %}
```csharp
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using Demos.Behaviours;
using UnityEngine;

namespace Demos.Actions
{
    [CreateAssetMenu(menuName = "Goap/Actions/EatAppleAction")]
    public class EatAppleAction : ActionBase<EatAppleAction.Data>
    {
        public override void OnStart(IMonoAgent agent, Data data)
        {
            if (data.Target is not TransformTarget)
                return;

            var inventory = agent.GetComponent<InventoryBehaviour>();

            if (inventory == null)
                return;
            
            data.Apple =  inventory.Get();
            data.Hunger = agent.GetComponent<HungerBehaviour>();
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data)
        {
            if (data.Apple == null || data.Hunger == null)
                return ActionRunState.Stop;

            var eatNutrition = Time.fixedDeltaTime * 10f;

            data.Apple.nutritionValue -= eatNutrition;
            data.Hunger.hunger -= eatNutrition;
            
            if (data.Apple.nutritionValue <= 0)
                GameObject.Destroy(data.Apple.gameObject);
            
            return ActionRunState.Continue;
        }
        
        public override void OnEnd(IMonoAgent agent, Data data)
        {
            if (data.Apple == null)
                return;
            
            var inventory = agent.GetComponent<InventoryBehaviour>();

            if (inventory == null)
                return;
            
            inventory.Put(data.Apple);
        }
        
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            public AppleBehaviour Apple { get; set; }
            public HungerBehaviour Hunger { get; set; }
        }
    }
}
```
{% endcode %}