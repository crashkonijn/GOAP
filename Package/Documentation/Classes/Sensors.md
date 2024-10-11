# Sensors

A `Sensor` is a class that reads the current state of the world and provides this information to the `WorldState` when it's needed.  The `Resolver` uses this information to determine the best action to perform based on the current state of the world.

Sensors can provide the values for two types of data/keys:
- **WorldKey**: A WorldKey references a value in the world. For example `AppleCount`. All values must be represented by `ints`.
- **TargetKey**: A TargetKey references a position in the world. For example `AppleTree`. All positions must be represented by `Vector3`.

Sensors can work in two scopes: `Global` or `Local`.

- **Global**: These sensors give information for all agents of an `AgentType`. For instance, `IsDaytimeSensor` checks if it's day or night for everyone.
- **Local**: They give information for just one agent. For example, `ClosestAppleSensor` finds the nearest apple for a specific agent.

|           | Local                 | Global                 |
|-----------|-----------------------|------------------------|
| WorldKey  | LocalWorldSensorBase  | GlobalWorldSensorBase  |
| TargetKey | LocalTargetSensorBase | GlobalTargetSensorBase |


![Sensor data flow](../images/sensor_flow.png)

## WorldSensor

`WorldSensor` checks the game's situation for an agent. It uses `WorldKey` to show each situation. The `Planner` uses this to pick the best action.

Examples:
- `IsHungrySensor` checks if the agent is hungry.
- `HasAppleSensor` checks if the agent has an apple.

### Example
To create a new `WorldSensor`, create a new class that inherits from `LocalWorldSensorBase` or `GlobalWorldSensorBase` and implement its `Sense` method.

{% code title="IsHungrySensor.cs" lineNumbers="true" %}
```csharp
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Demos.Simple.Behaviours;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.Demos.Simple.Goap.Sensors.World
{
    [GoapId("Simple-IsHungrySensor")]
    public class IsHungrySensor : LocalWorldSensorBase
    {
        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override SenseValue Sense(IActionReceiver agent, IComponentReference references)
        {
            var hungerBehaviour = references.GetCachedComponent<SimpleHungerBehaviour>();

            if (hungerBehaviour == null)
                return false;

            return hungerBehaviour.hunger > 20;
        }
    }
}
```
{% endcode %}

## TargetSensor

`TargetSensor` finds a position for a `TargetKey`. The `Planner` uses this to know how far actions are.

There are two kinds of `Target`: `TransformTarget` and `PositionTarget`.

- **TransformTarget**: Use this when the target can move. For example, `ClosestEnemySensor` finds a moving enemy.
- **PositionTarget**: Use this for a fixed spot. Like, `WanderTargetSensor` finds a random spot that doesn't move.

### Example
To create a new `TargetSensor`, create a new class that inherits from `LocalTargetSensorBase` or `GlobalTargetSensorBase` and implement its `Sense` method.

{% code title="ClosestTreeSensor.cs" lineNumbers="true" %}
```csharp
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Demos.Simple.Behaviours;
using CrashKonijn.Goap.Runtime;
using Demos;

namespace CrashKonijn.Goap.Demos.Simple.Goap.Sensors.Target
{
    [GoapId("Simple-ClosestTreeSensor")]
    public class ClosestTreeSensor : LocalTargetSensorBase
    {
        private TreeBehaviour[] trees;

        public override void Created()
        {            
            this.trees = Compatibility.FindObjectsOfType<TreeBehaviour>();
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IActionReceiver agent, IComponentReference references, ITarget target)
        {
            return new TransformTarget(this.trees.Closest(agent.Transform.position).transform);
        }
    }
}
```
{% endcode %}

{% code title="WanderTargetSensor.cs" lineNumbers="true" %}
```csharp
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Simple.Goap.Sensors.Target
{
    [GoapId("Simple-WanderTargetSensor")]
    public class WanderTargetSensor : LocalTargetSensorBase
    {
        private static readonly Vector2 Bounds = new Vector2(15, 8);

        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IActionReceiver agent, IComponentReference references, ITarget target)
        {
            var random = this.GetRandomPosition(agent);
            
            return new PositionTarget(random);
        }

        private Vector3 GetRandomPosition(IActionReceiver agent)
        {
            var random =  Random.insideUnitCircle * 5f;
            var position = agent.Transform.position + new Vector3(random.x, 0f, random.y);
            
            if (position.x > -Bounds.x && position.x < Bounds.x && position.z > -Bounds.y && position.z < Bounds.y)
                return position;

            return this.GetRandomPosition(agent);
        }
    }
}
```
{% endcode %}