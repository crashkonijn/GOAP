# Getting Started > Code

1. Create a new scene
2. Create a new GameObject called `Goap`, add the `GoapRunnerBehaviour` to it.

![Goap Runner Behaviour](../images/getting_started_goap_runner.png)

3. Create a class called `WanderTarget` that extends `TargetKeyBase`.

{% code title="WanderTarget.cs" lineNumbers="true" %}
```csharp
using CrashKonijn.Goap.Behaviours;

public class WanderTarget : TargetKeyBase
{
}
```
{% endcode %}

4. Create a class called `IsWandering` that extends `WorldKeyBase`.

{% code title="IsWandering.cs" lineNumbers="true" %}
```csharp
using CrashKonijn.Goap.Behaviours;

public class IsWandering : WorldKeyBase
{
}
```
{% endcode %}

5. Create a class called `GoapSetConfigFactory` that extends `GoapSetConfigFactoryBase` and override the `Create` method.

{% code title="GoapSetConfigFactory.cs" lineNumbers="true" %}
```csharp
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Resolver;

public class GoapSetConfigFactory : GoapSetFactoryBase
{
    public override IGoapSetConfig Create()
    {
        var builder = new GoapSetBuilder("GettingStartedSet");
        
        // Goals
        builder.AddGoal<WanderGoal>()
            .AddCondition<IsWandering>(Comparison.GreaterThanOrEqual, 1);

        // Actions
        builder.AddAction<WanderAction>()
            .SetTarget<WanderTarget>()
            .AddEffect<IsWandering>(true)
            .SetBaseCost(1)
            .SetInRange(0.3f);

        // Target Sensors
        builder.AddTargetSensor<WanderTargetSensor>()
            .SetTarget<WanderTarget>();

        // World Sensors
        // This example doesn't have any world sensors. Look in the examples for more information on how to use them.

        return builder.Build();
    }
}
```
{% endcode %}

6. Add the `GoapSetConfigFactory` to the `Goap` GameObject. Make sure to add the `GoapSetConfigFactory` to the `GoapRunnerBehaviour`'s factories property.

![Goap Runner](../images/getting_started_goap_runner_01.png)

7. Create a script called `GoapSetBinder`. This script will assign a `GoapSet` to the `Agent`.

{% code title="GoapSetBinder.cs" lineNumbers="true" %}
```csharp
using CrashKonijn.Goap.Behaviours;
using UnityEngine;

public class GoapSetBinder : MonoBehaviour {
    public void Awake() {
        var runner = FindObjectOfType<GoapRunnerBehaviour>();
        var agent = GetComponent<AgentBehaviour>();
        agent.GoapSet = runner.GetSet("GettingStartedSet");
    }
}
```
{% endcode %}

8. Create a sphere `GameObject` called `Agent`. Add the `AgentBehaviour`, `AgentMoveBehaviour`, `AgentBrain` and `GoapSetBinder` to the `GameObject`.

![Agent](../images/getting_started_agent.png)

9. Run the scene. The agent should move around randomly.