# Actions

In the GOAP system, an action represents a discrete step an agent can undertake to achieve a specific goal. Actions are defined by their requirements and effects, which guide the chaining of actions to form a plan.

## Components of an Action

Actions are composed of four primary parts:

1. **Config**: Configuration settings for the action.
2. **Action Class**: The logic and behavior of the action.
3. **Action Data**: Temporary data storage for the action's state.
4. **Action Props**: Additional properties for the action.

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

### StopppingDistance

This value specifies the proximity required between the agent and the target position before the action can commence.

### RequiresTarget

This value determines if a valid Target is required for the action to be executable.

### ValidateTarget

This value determines if the target is validated whilst running.

### ValidateConditions

This determines if the conditions of this action are validated whilst running.

## MoveMode

`MoveMode` determines how the action and movement are coordinated:

- **MoveBeforePerforming**: The agent moves to the target position before initiating the action.
- **PerformWhileMoving**: The agent concurrently moves to the target and executes the action.

## Action Data

Action data provides temporary storage for the action's state for an individual agent. This data is not shared across agents or across multiple invocations of the same action.

## Action Props

Action props are additional properties that can be used to customize the action's behavior. They are defined as fields in the action class and can be set as configuration values on ScriptableObjects or through code.

{% code lineNumbers="true" %}
```csharp
[Serializable]
public class Props : IActionProperties
{
    public float minTimer;
    public float maxTimer;
}
```
{% endcode %}

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

### IActionRunState Interface

The `IActionRunState` interface is a crucial component in the GOAP system. It defines the contract for action run states, which are responsible for determining the behavior of actions during their execution. These states decide when an action should be updated, stopped, performed, completed, or even resolved. They can also be used to 'pause' running an action, and come back later to continue it.

### Enabling/Disabling Actions
Each action can be enabled or disabled using the `action.Enable()` or `action.Disable(IActionDisabler)` methods. By default the following disablers are available:

{% code lineNumbers="true" %}
```csharp
public static class ActionDisabler
{
    public static IActionDisabler Forever => new ForeverActionDisabler();
    public static IActionDisabler ForTime(float time) => new ForTimeActionDisabler(time);
}
```
{% endcode %}

#### Examples

{% code lineNumbers="true" %}
```csharp
foreach (var pickupAppleAction in this.actionProvider.GetActions<PickupAppleAction>())
{
    pickupAppleAction.Disable(ActionDisabler.Forever);
}
```
{% endcode %}

{% code  lineNumbers="true" %}
```csharp
public class EatAction : GoapActionBase<EatAction.Data>
{
    // Other methods omitted for brevity
    public override void End(IMonoAgent agent, Data data)
    {
        // This will disable the action for 5 seconds
        this.Disable(ActionDisabler.ForTime(5f));
    }
}
```
{% endcode %}

#### Methods

- `void Update(IAgent agent, IActionContext context)`: Updates the state of the action based on the current context and agent state. This method is called every frame during the action's execution.

- `bool ShouldStop(IAgent agent)`: Determines whether the action should be stopped. If this method returns `true`, the action will be stopped.

- `bool ShouldPerform(IAgent agent)`: Determines whether the action should continue performing. If this method returns `true`, the action will continue its execution.

- `bool IsCompleted(IAgent agent)`: Checks if the action has been completed. If this method returns `true`, the action is considered completed.

- `bool MayResolve(IAgent agent)`: Determines whether the action may resolve based on the current state of the agent. This is used for actions that have conditional completion criteria.

- `bool IsRunning()`: Indicates whether the action is currently running. This can be used to check the state of the action outside of the usual update cycle.

#### Usage

`IActionRunState` allows for the creation of custom run states for actions, providing flexibility in how actions are executed within the GOAP system. By implementing this interface, developers can define custom logic for when actions should start, stop, or update, allowing for complex behavior patterns.

Several predefined action run states are provided out of the box, such as `Continue`, `Stop`, `Completed`, and various `Wait` states, to cover common use cases.

#### Example

Here's a simple example of a custom action run state that stops the action after a certain time has elapsed:

```csharp
public class TimedStopActionRunState : IActionRunState
{
    private float startTime;
    private float duration;

    public TimedStopActionRunState(float duration)
    {
        this.duration = duration;
    }

    public void Update(IAgent agent, IActionContext context)
    {
        if (startTime == 0)
            startTime = Time.time;
    }

    public bool ShouldStop(IAgent agent)
    {
        return Time.time - startTime >= duration;
    }

    public bool ShouldPerform(IAgent agent) => true;
    public bool IsCompleted(IAgent agent) => false;
    public bool MayResolve(IAgent agent) => true;
    public bool IsRunning() => true;
}
```

### Examples

The provided examples illustrate how to implement specific functionalities within the action class and action data. They've been retained in their original form for clarity.

### Examples
{% code title="ExampleAction.cs" lineNumbers="true" %}
```csharp
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Complex
{
    [GoapId("Example-93edf472-9fb5-4c55-84fa-5f6671992a6a")]
    public class ExampleAction : GoapActionBase<ExampleAction.Data>
    {
        // This method is called when the action is created
        // This method is optional and can be removed
        public override void Created()
        {
        }

        // This method is called every frame before the action is performed
        // If this method returns false, the action will be stopped
        // This method is optional and can be removed
        public override bool IsValid(IActionReceiver agent, Data data)
        {
            return true;
        }

        // This method is called when the action is started
        // This method is optional and can be removed
        public override void Start(IMonoAgent agent, Data data)
        {
        }

        // This method is called once before the action is performed
        // This method is optional and can be removed
        public override void BeforePerform(IMonoAgent agent, Data data)
        {
        }

        // This method is called every frame while the action is running
        // This method is required
        public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
        {
            return ActionRunState.Completed;
        }

        // This method is called when the action is completed
        // This method is optional and can be removed
        public override void Complete(IMonoAgent agent, Data data)
        {
        }

        // This method is called when the action is stopped
        // This method is optional and can be removed
        public override void Stop(IMonoAgent agent, Data data)
        {
        }

        // This method is called when the action is completed or stopped
        // This method is optional and can be removed
        public override void End(IMonoAgent agent, Data data)
        {
        }

        // The action class itself must be stateless!
        // All data should be stored in the data class
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
        }
    }
}
```
{% endcode %}

{% code title="WanderAction.cs" lineNumbers="true" %}
```csharp
using System;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using Random = UnityEngine.Random;

namespace CrashKonijn.Goap.Demos.Complex.Actions
{
    public class WanderAction : GoapActionBase<WanderAction.Data, WanderAction.Props>
    {
        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, Data data)
        {
            var wait = Random.Range(this.Properties.minTimer, this.Properties.maxTimer);
            
            data.Timer = ActionRunState.Wait(wait);
        }

        public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
        {
            if (data.Timer.IsRunning())
                return data.Timer;
            
            return ActionRunState.Completed;
        }

        public override void Stop(IMonoAgent agent, Data data)
        {
        }

        public override void Complete(IMonoAgent agent, Data data)
        {
        }

        [Serializable]
        public class Props : IActionProperties
        {
            public float minTimer;
            public float maxTimer;
        }

        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            public IActionRunState Timer { get; set; }
        }
    }
}
```
{% endcode %}
