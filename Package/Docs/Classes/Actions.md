# Actions

In the GOAP system, an action represents a discrete step an agent can undertake to achieve a specific goal. Actions are defined by their requirements and effects, which guide the chaining of actions to form a plan.

## Components of an Action

Actions are composed of three primary parts:

1. **Config**: Configuration settings for the action.
2. **Action Class**: The logic and behavior of the action.
3. **Action Data**: Temporary data storage for the action's state.

## Action Config

The configuration provides essential settings for the action, enabling its integration into the GOAP graph.

### Conditions

Conditions are a set of world states that must be met for the action to be executable. Each condition references a `WorldKey` and specifies whether its value should be true or false.

### Effects

Effects describe the changes in world states that result from performing the action. Each effect references a `WorldKey` and indicates the expected outcome (true or false).

### BaseCost

This represents the inherent cost of executing the action, excluding any additional costs (like distance) that the planner might add.

### Target

Every action has an associated target position. Before executing the action, the agent will move towards this target, depending on the `MoveMode`. Targets are identified using `TargetKey`, such as `ClosestApple` or `ClosestEnemy`.

### InRange

This value specifies the proximity required between the agent and the target position before the action can commence.

## MoveMode

`MoveMode` determines how the action and movement are coordinated:

- **MoveBeforePerforming**: The agent moves to the target position before initiating the action.
- **PerformWhileMoving**: The agent concurrently moves to the target and executes the action.

## Action Data

Action data provides temporary storage for the action's state for an individual agent. This data is not shared across agents or across multiple invocations of the same action.

### Action Data Injection

To reference other classes on the agent, use the `GetComponent` attribute. This provides a cached component instance, optimizing performance by avoiding frequent `GetComponent` calls.

{% code lineNumbers="true" %}
```csharp
public class Data : IActionData
{
    public ITarget Target { get; set; }
    
    [GetComponent]
    public ComplexInventoryBehaviour Inventory { get; set; }
}
```
{% endcode %}

## Action Class

The action class defines the behavior of the action. It should be stateless since a single instance might be used to execute the same action on different agents. The class inherits from `ActionBase<TData>`, where `TData` is the action data class.

### ActionRunState

This enum indicates the action's current state:

- **Continue**: The action will persist and be re-evaluated in the next frame.
- **Stop**: The action will terminate, and control will revert to the planner.

### Examples

The provided examples illustrate how to implement specific functionalities within the action class and action data. They've been retained in their original form for clarity.

### Examples
{% code title="WanderAction.cs" lineNumbers="true" %}
```csharp
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace Demos.Actions
{
    public class WanderAction : ActionBase<WanderAction.Data>
    {
        // You can implement a custom cost function. This is useful if you want to add dynamic costs to the action.
        public override float GetCost(IMonoAgent agent, IComponentReference references)
        {
            return 5f;
        }
        
        // This method is called to determine if the agent is in range for the action. It is called every frame while the action is running.
        // This could be used to perform a physics check to actually guarantee line of sight for example.
        public virtual bool IsInRange(IMonoAgent agent, float distance, IActionData data, IComponentReference references)
        {
            return distance <= this.config.InRange;
        }
    
        // This methods is called when the action is created. It is used to initialize the action.
        public override void Created()
        {
        }
    
        // This method is called when the action is started. It is used to initialize the action.
        public override void Start(IMonoAgent agent, Data data)
        {
        }

        // This method is called every frame while the action is running. It is used to perform the action.
        public override ActionRunState OnPerform(IMonoAgent agent, Data data, ActionContext context)
        {
            return ActionRunState.Stop;
        }

        // This method is called when the action is stopped. It is used to clean up the action.
        public override void End(IMonoAgent agent, Data data)
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
using Demos.Shared.Behaviours;
using Demos.Simple.Behaviours;
using UnityEngine;

namespace Demos.Simple.Actions
{
    public class EatAppleAction : ActionBase<EatAppleAction.Data>
    {
        public override void Created()
        {
        }
    
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

        public override ActionRunState OnPerform(IMonoAgent agent, Data data, ActionContext context)
        {
            if (data.Apple == null || data.Hunger == null)
                return ActionRunState.Stop;

            var eatNutrition = context.DeltaTime * 20f;

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
