# Getting Started

## Installation
Add the package to your project using the package manager. Add the following URL to the package manager:
```
https://github.com/crashkonijn/GOAP.git?path=/Package
```

## Setup in Unity

1. Create a class called `WanderGoal` that extends `GoalBase`.

{% code title="WanderGoal.cs" lang="csharp" %}
```csharp
public class WanderGoal : GoalBase
{
}
```
{% endcode %}

2. Create a class called `WanderAction` that extends `ActionBase`. The generic value of the class is the type of the data class used in this goal.

{% code title="WanderAction.cs" lang="csharp" %}
```csharp
public class WanderAction : ActionBase<WanderAction.Data>
{
    public override void Created()
    {
    }

    public override void Start(IMonoAgent agent, Data data)
    {
    }

    public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
    {
        return ActionRunState.Stop;
    }

    public override void End(IMonoAgent agent, Data data)
    {
    }

    public class Data : IActionData
    {
        public ITarget Target { get; set; }
    }
}
```
{% endcode %}

3. Create a class called `WanderTargetSensor` that extends `LocalTargetSensorBase`. The generic value of the class is the type of the data class used in this goal.

{% code title="WanderTargetSensor.cs" lang="csharp" %}
```csharp
public class WanderTargetSensor : LocalTargetSensorBase
{
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

        return this.GetRandomPosition(agent);
    }
}
```
{% endcode %}

4. Create a class called `AgentMoveBehaviour`. This class will be called by the `AgentBehaviour` to move the agent to a target.

{% code title="AgentMoveBehaviour.cs" lang="csharp" %}
```csharp
public class AgentMoveBehaviour : MonoBehaviour, IAgentMover
{
    private ITarget target;
    
    public void Move(ITarget target)
    {
        this.target = target;
        this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(this.target.Position.x, this.transform.position.y, this.target.Position.z), Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        if (this.target == null)
            return;
        
        Gizmos.DrawLine(this.transform.position, this.target.Position);
    }
}
```
{% endcode %}

5. Create a script called `AgentBrain`.

{% code title="AgentBrain.cs" lang="csharp" %}
```csharp
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