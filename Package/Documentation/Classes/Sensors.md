# Sensors

Sensors help the GOAP system understand the current game situation.

There are two main types of sensors: `WorldSensor` and `TargetSensor`.

## Global vs. Local Sensors

Sensors can work in two modes: `Global` or `Local`.

- **Global**: These sensors give information for all agents. For instance, `IsDaytimeSensor` checks if it's day or night for everyone.
- **Local**: These sensors check only when the `Planner` runs. They give information for just one agent. For example, `ClosestAppleSensor` finds the nearest apple for a specific agent.

## WorldSensor

`WorldSensor` checks the game's situation for an agent. It uses `WorldKey` to show each situation. The `Planner` uses this to pick the best action.

Examples:
- `IsHungrySensor` checks if the agent is hungry.
- `HasAppleSensor` checks if the agent has an apple.

### Example
To create a new `WorldSensor`, create a new class that inherits from `LocalWorldSensorBase` or `GlobalWorldSensorBase` and implement its `Sense` method.

{% code title="IsHungrySensor.cs" lineNumbers="true" %}
```csharp
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Sensors;
using Demos.Shared.Behaviours;

namespace Demos.Simple.Sensors.World
{
    public class IsHungrySensor : LocalWorldSensorBase
    {
        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            // References are cached by the agent.
            var hungerBehaviour = references.GetComponent<HungerBehaviour>();

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

{% code title="ClosestAppleSensor.cs" lineNumbers="true" %}
```csharp
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Demos.Simple.Behaviours;
using UnityEngine;

namespace Demos.Simple.Sensors.Target
{
    public class ClosestAppleSensor : LocalTargetSensorBase
    {
        private AppleCollection apples;

        public override void Created()
        {
            this.apples = GameObject.FindObjectOfType<AppleCollection>();
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            var closestApple = this.apples.Get().Closest(agent.transform.position);

            if (closestApple is null)
                return null;
            
            return new TransformTarget(closestApple.transform);
        }
    }
}
```
{% endcode %}

{% code title="WanderTargetSensor.cs" lineNumbers="true" %}
```csharp
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using UnityEngine;

namespace Demos.Simple.Sensors.Target
{
    public class WanderTargetSensor : LocalTargetSensorBase
    {
        private static readonly Vector2 Bounds = new Vector2(15, 8);

        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            var random = this.GetRandomPosition(agent);
            
            return new PositionTarget(random);
        }

        private Vector3 GetRandomPosition(IMonoAgent agent)
        {
            var random =  Random.insideUnitCircle * 5f;
            var position = agent.transform.position + new Vector3(random.x, 0f, random.y);
            
            if (position.x > -Bounds.x && position.x < Bounds.x && position.z > -Bounds.y && position.z < Bounds.y)
                return position;

            return this.GetRandomPosition(agent);
        }
    }
}
```
{% endcode %}