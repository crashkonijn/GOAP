# Sensors
Sensors are classes that can determine the state that your game is in.

There are two types of sensors: `WorldSensor` and `TargetSensor`.

## Global vs Local
Each sensor operates either in `Local` or `Global` mode.

### Global
Global sensors are run each frame. They are used to determine a state for all agents. For example, the `IsDaytimeSensor` determines whether it is daytime or not. This state is shared between all agents.

### Local
Local sensors are run before the `Planner` is run. They are used to determine a state for a single agent. For example, the `ClosestAppleSensor` determines the position of the closest apple to a specific `Agent`.

## WorldSensor
A `WorldSensor` is used to determine the state of the world for an `Agent`. Each state is represented by a `WorldKey`. The WorldState is used by the `Planner` to determine the best `Action` for the current `Goal`.

For example:
- The `IsHungrySensor` determines the `IsHungry` state of the `Agent`.
- The `HasAppleSensor` determines the `HasApple` state of the `Agent`.

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
A `TargetSensor` is used to determine the position for a given `TargetKey`. The TargetState is used by the `Planner` to determine distance between `Actions`.

For example:
- The `ClosestAppleSensor` determines the position of the closest apple.
- The `ClosestTreeSensor` determines the position of the closest tree.

### TransformTarget vs PositionTarget
There are two types of `Target`: `TransformTarget` and `PositionTarget`.

#### TransformTarget
`TransformTarget` is used when you want to move to a specific `Transform` that may change position over time. For example, the `ClosestEnemySensor` returns a `TransformTarget` because the enemy may move.

#### PositionTarget
`PositionTarget` is used when you want to move to a specific position that will not change. For example, the `WanderTargetSensor` returns a `PositionTarget` because the position is randomly generated and doesn't change.

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