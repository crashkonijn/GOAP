# Upgrading

## Upgrading from 2.0 to 2.1

### IAgentMover is removed
`IAgentMover` is removed in favor of having movement based events on the agent.

{% code title="AgentMoveBehaviour.cs" lineNumbers="true" %}
```csharp
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

public class AgentMoveBehaviour : MonoBehaviour
{
    private AgentBehaviour agent;
    private ITarget currentTarget;
    private bool shouldMove;

    private void Awake()
    {
        this.agent = this.GetComponent<AgentBehaviour>();
    }

    private void OnEnable()
    {
        this.agent.Events.OnTargetInRange += this.OnTargetInRange;
        this.agent.Events.OnTargetChanged += this.OnTargetChanged;
        this.agent.Events.OnTargetOutOfRange += this.OnTargetOutOfRange;
    }

    private void OnDisable()
    {
        this.agent.Events.OnTargetInRange -= this.OnTargetInRange;
        this.agent.Events.OnTargetChanged -= this.OnTargetChanged;
        this.agent.Events.OnTargetOutOfRange -= this.OnTargetOutOfRange;
    }

    private void OnTargetInRange(ITarget target)
    {
        this.shouldMove = false;
    }

    private void OnTargetChanged(ITarget target, bool inRange)
    {
        this.currentTarget = target;
        this.shouldMove = !inRange;
    }

    private void OnTargetOutOfRange(ITarget target)
    {
        this.shouldMove = true;
    }

    public void Update()
    {
        if (!this.shouldMove)
            return;
        
        if (this.currentTarget == null)
            return;
        
        this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(this.currentTarget.Position.x, this.transform.position.y, this.currentTarget.Position.z), Time.deltaTime);
    }
}
```
{% endcode %}

### Setup through code now requires actual classes as the WorldKey and TargetKey.

{% code lineNumbers="true" %}
```csharp
public class WanderTarget : TargetKeyBase
{
}

public class IsWandering : WorldKeyBase
{
}

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