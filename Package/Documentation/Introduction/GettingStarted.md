# Getting Started

## Installation
Add the package to your project using the package manager. Add the following URL to the package manager:
```
https://github.com/crashkonijn/GOAP.git?path=/Package
```

Alternatively install through [OpenUPM](https://openupm.com/packages/com.crashkonijn.goap/) or the [Unity Asset Store](https://assetstore.unity.com/packages/slug/252687).

{% hint style="info" %}
**Version** This package was build using unity 2022.2, but also confirmed to be working with 2021.3.
{% endhint %}

## Setup in Unity

1. Create a class called `WanderGoal` that extends `GoalBase`.

{% code title="WanderGoal.cs" lang="csharp" %}
```csharp
using CrashKonijn.Goap.Behaviours;

public class WanderGoal : GoalBase
{
}
```
{% endcode %}

2. Create a class called `WanderAction` that extends `ActionBase`. The generic value of the class is the type of the data class used in this goal.

{% code title="WanderAction.cs" lang="csharp" %}
```csharp
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

public class WanderAction : ActionBase<WanderAction.Data>
{
    // Called when the class is created.
    public override void Created()
    {
    }

    // Called when the action is started for a specific agent.
    public override void Start(IMonoAgent agent, Data data)
    {
        // When the agent is at the target, wait a random amount of time before moving again.
        data.Timer = Random.Range(0.3f, 1f);
    }

    // Called each frame when the action needs to be performed. It is only called when the agent is in range of it's target.
    public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
    {
        // Update timer.
        data.Timer -= context.DeltaTime;
        
        // If the timer is still higher than 0, continue next frame.
        if (data.Timer > 0)
            return ActionRunState.Continue;
        
        // This action is done, return stop. This will trigger the resolver for a new action.
        return ActionRunState.Stop;
    }

    // Called when the action is ended for a specific agent.
    public override void End(IMonoAgent agent, Data data)
    {
    }

    public class Data : IActionData
    {
        public ITarget Target { get; set; }
        public float Timer { get; set; }
    }
}
```
{% endcode %}

3. Create a class called `WanderTargetSensor` that extends `LocalTargetSensorBase`. The generic value of the class is the type of the data class used in this goal.

{% code title="WanderTargetSensor.cs" lang="csharp" %}
```csharp
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;

public class WanderTargetSensor : LocalTargetSensorBase
{
    // Called when the class is created.
    public override void Created()
    {
    }

    // Called each frame. This can be used to gather data from the world before the sense method is called.
    // This can be used to gather 'base data' that is the same for all agents, and otherwise would be performed multiple times during the Sense method.
    public override void Update()
    {
    }

    // Called when the sensor needs to sense a target for a specific agent.
    public override ITarget Sense(IMonoAgent agent, IComponentReference references)
    {
        var random = this.GetRandomPosition(agent);
        
        return new PositionTarget(random);
    }

    private Vector3 GetRandomPosition(IMonoAgent agent)
    {
        var random =  Random.insideUnitCircle * 5f;
        var position = agent.transform.position + new Vector3(random.x, 0f, random.y);

        return position;
    }
}
```
{% endcode %}

4. Create a class called `AgentMoveBehaviour`. This class will be called by the `AgentBehaviour` to move the agent to a target.

{% code title="AgentMoveBehaviour.cs" lang="csharp" %}
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

5. Create a script called `AgentBrain`.

{% code title="AgentBrain.cs" lang="csharp" %}
```csharp
using CrashKonijn.Goap.Behaviours;
using UnityEngine;

public class AgentBrain : MonoBehaviour
{
    private AgentBehaviour agent;

    private void Awake()
    {
        this.agent = this.GetComponent<AgentBehaviour>();
    }

    private void Start()
    {
        this.agent.SetGoal<WanderGoal>(false);
    }
}
```
{% endcode %}

## Choose your config style
Either continue the getting started by using `Code` or `ScriptableObjects`.